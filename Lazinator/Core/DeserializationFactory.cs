using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lazinator.Buffers;
using Lazinator.Exceptions;
using Lazinator.Support;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    [Serializable]
    public class DeserializationFactory
    {

        #region Construction

        static object LockObj = new object();
        static DeserializationFactory _Instance;
        public static DeserializationFactory Instance
        {
            get
            {
                if (_Instance != null)
                    return _Instance;
                lock (LockObj)
                {
                    if (_Instance == null)
                        // identify a type in each assembly where we want to load all types
                        _Instance = new DeserializationFactory();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// A dictionary with a key equal to a unique ID and the value equal to a factory that creates the object with that unique ID. This can be used only for types that have no open generic parameters. Types with properties that contain closed generic parameters can be included.
        /// </summary>
        private Dictionary<int, Func<ILazinator>> FactoriesByID =
            new Dictionary<int, Func<ILazinator>>();

        private ConcurrentDictionary<LazinatorGenericIDType, Func<ILazinator>> GenericFactories = new ConcurrentDictionary<LazinatorGenericIDType, Func<ILazinator>>();

        /// <summary>
        /// A dictionary with a key equal to a unique ID of a Lazinator type and a value recording the type and the number of generic parameters.
        /// </summary>
        private Dictionary<int, (Type type, byte numberGenericParameters)> UniqueIDToTypeMap = new Dictionary<int, (Type type, byte numberGenericParameters)>();

        private Dictionary<Type, int> TypeToUniqueIDMap = new Dictionary<Type, int>();

        private Dictionary<Type, int?> FixedUniqueIDs = new Dictionary<Type, int?>();

        private ConcurrentDictionary<Type, bool> NonBinaryHashing = new ConcurrentDictionary<Type, bool>();

        public DeserializationFactory() : this(AppDomain.CurrentDomain.GetAssemblies().ToArray())
        {

        }

        /// <summary>
        /// Construct a deserialization factory, considering all types in the specified assemblies
        /// </summary>
        /// <param name="assemblies">The assemblies in which to search for Lazinator types</param>
        public DeserializationFactory(Assembly[] assemblies)
        {
            SetupAllTypesInAssemblies(assemblies);
        }

        #endregion

        #region Factory creation

        private static Func<ILazinator> GetCompiledFunctionForType(Type t)
        {
            var exnew = Expression.New(t);
            var exconv = Expression.Convert(exnew, typeof(ILazinator));
            var lambda = Expression.Lambda(exconv);
            Delegate compiled = lambda.Compile();
            var result = (Func<ILazinator>)compiled;
            return result;
        }

        /// <summary>
        /// Create a typed Lazinator object of a certain concrete type. If the object if of that exact type rather than of a subtype, performance will be better than if the factory creation methods are used. This method should not be used for deserializing a struct or sealed class, since the serialized bytes may not contain object IDs.
        /// </summary>
        /// <typeparam name="T">The type of the object to be created</typeparam>
        /// <param name="mostLikelyUniqueID">The expected Lazinator ID of the object. </param>
        /// <param name="funcToCreateBaseType">A function to create an uninitialized object of the expected type, typically a function that simply calls the parameterless constructor and returns the result. Use of this function can help avoid the need for the factory creation methods and boxing of structs.</param>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        /// <returns></returns>
        public T CreateBaseOrDerivedType<T>(int mostLikelyUniqueID, Func<T> funcToCreateBaseType, LazinatorMemory storage, ILazinator parent = null) where T : ILazinator, new()
        {
            // Note: It's important that CreateBaseOrDerivedType be generic, because that often allows us to avoid boxing if the object being created is a struct and the uniqueID matches the mostLikelyUniqueID. 
            if (storage.IsEmpty || storage.Length == 1)
                return default;
            int bytesSoFar = 0;
            var span = (ReadOnlySpan<byte>)storage.Memory.Span;
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            T itemToReturn;
            if (!UniqueIDToTypeMap.ContainsKey(uniqueID))
                throw new LazinatorDeserializationException($"Could not deserialize, because unique ID {uniqueID} was found in the memory to be deserialized, but this unique ID is not known. This could arise by creating a Lazinator object and then deserializing it in an assembly where that object type is not present. "); // If there is a Lazinator internal error causing the unique ID to be read in the wrong place, that could also cause the error.
            var typeInfo = UniqueIDToTypeMap[uniqueID];
            if (typeInfo.numberGenericParameters > 0)
            {
                bytesSoFar = 0;
                LazinatorGenericIDType genericID = ReadLazinatorGenericID(span, ref bytesSoFar);
                itemToReturn = (T)CreateGenericKnownID(genericID);
            }
            else
            {
                if (uniqueID == mostLikelyUniqueID)
                {
                    itemToReturn = funcToCreateBaseType();
                }
                else
                {
                    if (FactoriesByID.ContainsKey(uniqueID))
                    {
                        itemToReturn = (T)FactoriesByID[uniqueID]();
                    }
                    else
                        throw new UnknownSerializedTypeException(uniqueID);
                }
            }
            InitializeDeserialized(itemToReturn, storage, parent);
            return itemToReturn;
        }

        /// <summary>
        /// Create a typed Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List.
        /// </summary>
        /// <typeparam name="T">Type of the item to create.</typeparam>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        public T CreateAbstractType<T>(LazinatorMemory storage, ILazinator parent = null) where T : ILazinator => (T)CreateFromBytesIncludingID(storage, parent);

        /// <summary>
        /// Create a Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List. This assumes that the serialized bytes contain type information.
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        public ILazinator CreateFromBytesIncludingID(LazinatorMemory storage, ILazinator parent = null)
        {
            if (storage.Memory.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = ((ReadOnlySpan<byte>)storage.Memory.Span).ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = CreateKnownID(uniqueID, storage, parent);
            InitializeDeserialized(itemToReturn, storage, parent);
            return itemToReturn;
        }

        /// <summary>
        /// Create a Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. The ID is passed as a parameter and need not be contained within the bytes themselves.
        /// </summary>
        /// <param name="uniqueID">The type of the item to create</param>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        /// <returns>The deserialized Lazinator object</returns>
        public ILazinator CreateKnownID(int uniqueID, LazinatorMemory storage, ILazinator parent = null)
        {
            ILazinator lazinatorObject = null;
            if (FactoriesByID.ContainsKey(uniqueID))
                lazinatorObject = FactoriesByID[uniqueID]();
            else
            {
                (Type t, int numGenericParameters) = UniqueIDToTypeMap[uniqueID];
                if (numGenericParameters > 0)
                    lazinatorObject = CreateGenericFromBytesIncludingID(storage.Memory.Span);
            }
            if (lazinatorObject == null)
                throw new UnknownSerializedTypeException(uniqueID);
            InitializeDeserialized(lazinatorObject, storage, parent);
            return lazinatorObject;
        }

        private void InitializeDeserialized(ILazinator lazinatorType, LazinatorMemory serializedBytes, ILazinator parent)
        {
            lazinatorType.LazinatorParents = new LazinatorParentsCollection(parent);
            lazinatorType.DeserializeLazinator(serializedBytes);
            lazinatorType.HasChanged = false;
            lazinatorType.DescendantHasChanged = false;
        }

        /// <summary>
        /// Create a Lazinator item from bytes, where the Lazinator item is known to be a particular type, which may be a type that does not preserve unique ID information in serialized bytes. This method checks whether the type is such a type. If so, the unique ID is known, and the item is created. Otherwise, the unique ID is in the serialized bytes, and the item is instantiated based on that. This approach is slower than reading the type from the serialized bytes, and it should thus be used only where it is not known at compile time what the type is.
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        /// <returns>The deserialized Lazinator object</returns>
        public T CreateBasedOnType<T>(LazinatorMemory storage, ILazinator parent = null)
        {
            Type t = typeof(T);
            int? fixedUniqueID = GetFixedUniqueID(t);
            if (fixedUniqueID != null)
                return (T)CreateKnownID((int)fixedUniqueID, storage, parent);
            else
                return (T)CreateFromBytesIncludingID(storage, parent);
        }

        /// <summary>
        /// Enumerate multiple objects stored successively in a single range of bytes and not in a Lazinator or .Net collection. Storing objects this way is useful when each is appended to the end of the storage. 
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="fixedUniqueID">The fixed unique ID of a single type representing each instance stored. If null is specified, then each stored object must contain a unique ID.</param>
        /// <returns></returns>
        public IEnumerable<ILazinator> CreateMultiple(LazinatorMemory storage, int? fixedUniqueID)
        {
            while (true) // until "yield break"
            {
                if (storage.Length <= 1)
                    yield break;
                int bytesSoFar = 0;
                int uniqueID = fixedUniqueID ?? storage.ReadOnlySpan.ToDecompressedInt(ref bytesSoFar);
                ILazinator itemToReturn = CreateKnownID(uniqueID, storage, null);
                int bytes = itemToReturn.LazinatorMemoryStorage.Length;
                storage = storage.Slice(bytes);
                yield return itemToReturn;
            }
        }

        #endregion

        #region Factory setup



        private void SetupAllTypesInAssemblies(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
                SetupAllTypesInAssembly(assembly);
        }

        private void SetupAllTypesInAssembly(Assembly assembly)
        {
            Type[] types = GetLoadableTypes(assembly)
                .Select(x => x.GetTypeInfo())
                .Where(x => x.ImplementedInterfaces.Contains(typeof(ILazinator)))
                //.Where(x => !x.IsInterface && !x.IsAbstract)
                .Select(x => x.AsType())
                .ToArray();
            SetTypes(types);
        }

        // see https://stackoverflow.com/questions/7889228/how-to-prevent-reflectiontypeloadexception-when-calling-assembly-gettypes
        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        const int AddThisWhenSerializingExclusiveInterface = 1000000000;

        private void SetTypes(Type[] typesImplementingILazinator)
        {
            typesImplementingILazinator = typesImplementingILazinator.Distinct().ToArray();
            foreach (Type type in typesImplementingILazinator)
            {
                int uniqueID;
                if (type.IsInterface)
                {
                    var attribute = LazinatorReflection.GetLazinatorAttributeForInterface(type);
                    if (attribute == null)
                    {
                        if (type.IsInterface)
                        {
                            var nonexclusiveAttribute = LazinatorReflection.GetNonexclusiveLazinatorAttributeForInterface(type);
                            if (nonexclusiveAttribute == null)
                                continue;
                            uniqueID = nonexclusiveAttribute.UniqueID;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        uniqueID = attribute.UniqueID + AddThisWhenSerializingExclusiveInterface;
                        NonBinaryHashing[type] = LazinatorReflection.InterfaceHasUseNonbinaryHashAttribute(type);
                    }
                }
                else
                {
                    var attribute = LazinatorReflection.GetLazinatorAttributeForLazinatorMainType(type);
                    if (attribute == null)
                        continue;
                    NonBinaryHashing[type] = LazinatorReflection.CorrespondingInterfaceHasUseNonbinaryHashAttribute(type);
                    uniqueID = attribute.UniqueID;
                }
                TypeToUniqueIDMap[type] = uniqueID;
                if (FactoriesByID.ContainsKey(uniqueID))
                    throw new LazinatorDeserializationException($"The type {type} has self-serialization UniqueID of {uniqueID}, but that {uniqueID} is already used by {FactoriesByID[uniqueID]().GetType()}.");
                if (type.ContainsGenericParameters)
                {
                    var genericArguments = type.GetGenericArguments().ToList();
                    var genericArgumentsConstrained = genericArguments.Where(x => x.GetGenericParameterConstraints().Any(y => y.Equals(typeof(Lazinator.Core.ILazinator)))).ToList();
                    if (genericArguments.Count() == genericArgumentsConstrained.Count())
                        UniqueIDToTypeMap[uniqueID] = (type, (byte)genericArguments.Count());
                    // otherwise, we have non-Lazinator type arguments, which we can't handle, and thus we can't add this to the dictionary. 
                }
                else
                {
                    UniqueIDToTypeMap[uniqueID] = (type, 0);
                    if (!type.IsInterface && !type.IsAbstract)
                    {
                        FactoriesByID[uniqueID] = GetCompiledFunctionForType(type);
                        int? fixedUniqueID = GetFixedUniqueIDOrNull(type);
                        if (fixedUniqueID != null)
                        {
                            FixedUniqueIDs[type] = (int)fixedUniqueID;
                            Type exclusiveInterface = LazinatorReflection.GetCorrespondingExclusiveInterface(type);
                            if (exclusiveInterface != null)
                                FixedUniqueIDs[exclusiveInterface] = (int)fixedUniqueID;
                        }
                    }
                }
            }
        }

        public int? GetFixedUniqueID(Type type)
        {
            if (FixedUniqueIDs.ContainsKey(type))
                return FixedUniqueIDs[type];
            return null;
        }

        private static int? GetFixedUniqueIDOrNull(Type t)
        {
            // Explanation: Most Lazinator objects include UniqueIDs, but some skip them. Usually, we know what we're dealing with, because we are serializing a property of a particular type. If the type is sealed or a struct, we just create the object directly, and then call its deserialization routines; otherwise, we call the factory, and we can be sure that there is a unique ID. But here, we seek to be able to deserialize any type, regardless of whether it includes a UniqueID.
            bool serializationSkipsUniqueIDs = t.IsValueType || (t.IsSealed && t.BaseType == typeof(object));
            int? fixedUniqueID = null;
            if (serializationSkipsUniqueIDs == true)
            {
                Attributes.LazinatorAttribute lazinatorAttribute = (Attributes.LazinatorAttribute)
                    t.GetInterfaces()
                    .Select(x => (x, x.GetCustomAttributes(typeof(Attributes.LazinatorAttribute), false)))
                    .Where(x => x.Item2.Any())
                    .FirstOrDefault()
                    .Item2
                    .FirstOrDefault();
                serializationSkipsUniqueIDs = lazinatorAttribute.Version == -1;
                if (serializationSkipsUniqueIDs == true)
                {
                    // We also can't skip a unique ID if the type has a nonexclusive interface.
                    Attributes.NonexclusiveLazinatorAttribute nonexclusiveAttribute = (Attributes.NonexclusiveLazinatorAttribute)
                    t.GetInterfaces()
                    .Select(x => (x, x.GetCustomAttributes(typeof(Attributes.NonexclusiveLazinatorAttribute), true)))
                    .Where(x => x.Item2.Any())
                    .FirstOrDefault()
                    .Item2
                    ?.FirstOrDefault();

                    if (nonexclusiveAttribute == null)
                        fixedUniqueID = lazinatorAttribute.UniqueID;
                }
            }

            return fixedUniqueID;
        }

        public bool HasNonBinaryHashAttribute(Type t)
        {
            Type declaringType = t.DeclaringType ?? t;
            if (NonBinaryHashing.ContainsKey(declaringType))
                return NonBinaryHashing[declaringType];
            bool result = t.IsInterface ? LazinatorReflection.InterfaceHasUseNonbinaryHashAttribute(declaringType) : LazinatorReflection.CorrespondingInterfaceHasUseNonbinaryHashAttribute(declaringType);
            NonBinaryHashing[declaringType] = result;
            return result;
        }

        #endregion

        #region Generic composition

        private ILazinator CreateGenericFromBytesIncludingID(ReadOnlySpan<byte> span)
        {
            int index = 0;
            LazinatorGenericIDType id = ReadLazinatorGenericID(span, ref index);
            return CreateGenericKnownID(id);
        }

        private ILazinator CreateGenericKnownID(LazinatorGenericIDType typeAndGenericTypeArgumentIDs)
        {
            Func<ILazinator> func = GetGenericFactoryBasedOnGenericIDType(typeAndGenericTypeArgumentIDs);
            return func();
        }



        public LazinatorGenericIDType GetUniqueIDListForGenericType(int outerTypeUniqueID, IEnumerable<Type> innerTypes)
        {
            List<int> l = new List<int>() { outerTypeUniqueID };
            foreach (Type t in innerTypes)
                AddToUniqueIDListForGenericType(t, l);
            return new LazinatorGenericIDType(l);
        }

        public List<int> GetUniqueIDListForGenericType(Type t)
        {
            List<int> l = new List<int>();
            AddToUniqueIDListForGenericType(t, l);
            return l;
        }

        private void AddToUniqueIDListForGenericType(Type t, List<int> l)
        {
            if (t.GenericTypeArguments.Any())
            {
                Type genericTypeDefinition = t.GetGenericTypeDefinition();
                if (TypeToUniqueIDMap.ContainsKey(genericTypeDefinition))
                {
                    int uniqueID = TypeToUniqueIDMap[genericTypeDefinition];
                    l.Add(uniqueID);
                    foreach (Type genericTypeArgument in t.GetGenericArguments())
                    {
                        AddToUniqueIDListForGenericType(genericTypeArgument, l);
                    }
                }
            }
            else
                l.Add(TypeToUniqueIDMap[t]);
        }

        public Func<ILazinator> GetGenericFactoryBasedOnGenericIDType(LazinatorGenericIDType typeAndGenericTypeArgumentIDs)
        {
            // NOTE: If this code causes a stack overflow error, it is likely because a property is being accessed in the constructor
            // before deserialization.
            if (GenericFactories.ContainsKey(typeAndGenericTypeArgumentIDs))
                return GenericFactories[typeAndGenericTypeArgumentIDs];
            (Type type, int numberTypeArgumentsConsumed) = GetTypeBasedOnGenericIDType(typeAndGenericTypeArgumentIDs);
            var factory = GetCompiledFunctionForType(type);
            GenericFactories[typeAndGenericTypeArgumentIDs] = factory;
            return factory;
        }

        public (Type type, int numberTypeArgumentsConsumed) GetTypeBasedOnGenericIDType(LazinatorGenericIDType typeAndGenericTypeArgumentIDs, int index = 0)
        {
            if (index >= typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs.Count)
                throw new LazinatorDeserializationException($"Unexpected exception deserializing type with unique ID {typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs[0]}. The type ID consisted of {typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs.Count} integer IDs, but more than that was needed.");
            int uniqueIDOfMainType = typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs[index];
            if (UniqueIDToTypeMap.ContainsKey(uniqueIDOfMainType))
            {
                (Type t, byte numberGenericParameters) = UniqueIDToTypeMap[uniqueIDOfMainType];
                int numberTypeArgumentsConsumed = 1;
                if (numberGenericParameters == 0)
                    return (t, numberTypeArgumentsConsumed);
                Type[] typeParameters = new Type[numberGenericParameters];
                for (int gp = 0; gp < numberGenericParameters; gp++)
                {
                    (Type genericParameterType, int moreTypeArgumentsConsumed) = GetTypeBasedOnGenericIDType(typeAndGenericTypeArgumentIDs, index + numberTypeArgumentsConsumed);
                    numberTypeArgumentsConsumed += moreTypeArgumentsConsumed;
                    typeParameters[gp] = genericParameterType;
                }
                Type result = t.MakeGenericType(typeParameters);
                if (index == 0 && numberTypeArgumentsConsumed != typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs.Count())
                    throw new LazinatorDeserializationException($"Unexpected exception deserializing type with unique ID {typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs[0]}. The type ID consisted of {typeAndGenericTypeArgumentIDs.TypeAndInnerTypeIDs.Count} integer IDs, but only {numberTypeArgumentsConsumed} were needed.");
                return (result, numberTypeArgumentsConsumed);
            }
            else
                throw new LazinatorDeserializationException($"Could not deserialize type with unique ID {uniqueIDOfMainType}. Perhaps it is a type in an assembly not initialized by the DeserializationFactory, or perhaps it is a type with non-Lazinator generic parameters?");
        }

        #endregion
    }
}
