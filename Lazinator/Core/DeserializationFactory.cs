using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lazinator.Buffers; 
using Lazinator.Core;
using Lazinator.Exceptions;
using Lazinator.Support;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    [Serializable]
    public class DeserializationFactory
    {

        static object LockObj = new object();
        static DeserializationFactory _DeserializationFactory;
        public static DeserializationFactory GetInstance()
        {
            if (_DeserializationFactory != null)
                return _DeserializationFactory;
            lock (LockObj)
            {
                if (_DeserializationFactory == null)
                    // identify a type in each assembly where we want to load all types
                    _DeserializationFactory = new DeserializationFactory();
            }
            return _DeserializationFactory;
        }

        private Dictionary<int, Func<ILazinator>> FactoriesByID =
            new Dictionary<int, Func<ILazinator>>();

        private Dictionary<Type, int?> FixedUniqueIDs = new Dictionary<Type, int?>();

        public DeserializationFactory() : this(AppDomain.CurrentDomain.GetAssemblies().ToArray())
        {

        }

        /// <summary>
        /// Construct a deserialization factory that is aware of all Lazinator types in the same assembly as a specified type.
        /// </summary>
        /// <param name="type">A type used to identify an assembly</param>
        public DeserializationFactory(Type type) : this(new Type[] {type}, true)
        {

        }

        /// <summary>
        /// Construct a deserialization factory
        /// </summary>
        /// <param name="types">Types the deserialization factory should be aware of</param>
        /// <param name="loadAllEligibleTypesInCorrespondingAssemblies">Whether the deserialization factory should also consider all other types in the same assemblies as the specified types</param>
        public DeserializationFactory(Type[] types, bool loadAllEligibleTypesInCorrespondingAssemblies)
        {
            if (!loadAllEligibleTypesInCorrespondingAssemblies)
                SetTypes(types);
            else
                SetupAllTypesInAssemblies(types.Select(x => x.Assembly).Distinct().ToArray());
        }

        /// <summary>
        /// Construct a deserialization factory, considering all types in the specified assemblies
        /// </summary>
        /// <param name="assemblies">The assemblies in which to search for Lazinator types</param>
        public DeserializationFactory(Assembly[] assemblies)
        {
            SetupAllTypesInAssemblies(assemblies);
        }

        /// <summary>
        /// Create a typed Lazinator object of a certain concrete type. If the object if of that exact type rather than of a subtype, performance will be better than if the factory creation methods are used.
        /// </summary>
        /// <typeparam name="T">The type of the object to be created</typeparam>
        /// <param name="mostLikelyUniqueID">The expected Lazinator ID of the object. </param>
        /// <param name="funcToCreateMostLikely">A function to create an uninitialized object of the expected type, typically a function that simply calls the parameterless constructor and returns the result. Use of this function can help avoid the need for the factory creation methods and boxing of structs.</param>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        /// <returns></returns>
        public T Create<T>(int mostLikelyUniqueID, Func<T> funcToCreateMostLikely, ReadOnlyMemory<byte> storage, ILazinator parent = null) where T : ILazinator, new()
        {
            // Note: It's important that Create be generic, because that allows us to avoid boxing if the object being created is a struct and the uniqueID matches the mostLikelyUniqueID. 
            if (storage.Length <= 1)
                return default;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            T itemToReturn;
            if (uniqueID == mostLikelyUniqueID)
            {
                itemToReturn = funcToCreateMostLikely();
                InitializeDeserialized(itemToReturn, storage, parent);
            }
            else
                itemToReturn = (T) FactoryCreate(uniqueID, storage, parent); // we'll have to box structs here. We can't get around it because our dictionary's values are Func<ILazinator>, so we're going to have to cast that. We could alternatively have a dictionary of dictionaries indexed by type, but that might have a significant performance cost as well.
            return itemToReturn;
        }

        /// <summary>
        /// Create a typed Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List.
        /// </summary>
        /// <typeparam name="T">Type of the item to create.</typeparam>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        public T FactoryCreate<T>(ReadOnlyMemory<byte> storage, ILazinator parent) where T : ILazinator, new() => (T)FactoryCreate(storage, parent);

        /// <summary>
        /// Create a Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List.
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        public ILazinator FactoryCreate(ReadOnlyMemory<byte> storage, ILazinator parent)
        {
            if (storage.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = FactoryCreate(uniqueID, storage, parent);
            return itemToReturn;
        }
        
        /// <summary>
        /// Create a Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List.
        /// </summary>
        /// <param name="uniqueID">The type of the item to create</param>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        /// <returns>The deserialized Lazinator object</returns>
        public ILazinator FactoryCreate(int uniqueID, ReadOnlyMemory<byte> storage, ILazinator parent)
        {
            if (FactoriesByID.ContainsKey(uniqueID))
            {
                ILazinator selfSerialized = FactoriesByID[uniqueID]();
                InitializeDeserialized(selfSerialized, storage, parent);
                return selfSerialized;
            }
            else
                throw new UnknownSerializedTypeException(uniqueID);
        }

        private void InitializeDeserialized(ILazinator lazinatorType, ReadOnlyMemory<byte> serializedBytes, ILazinator parent)
        {
            lazinatorType.DeserializationFactory = this;
            lazinatorType.LazinatorParentClass = parent;
            lazinatorType.LazinatorObjectBytes = serializedBytes;
        }

        /// <summary>
        /// Create a Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List.
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="informParentOfDirtinessDelegate">A delegate to be called when the item has changed.</param>
        /// <returns>The deserialized Lazinator object</returns>
        public ILazinator FactoryCreate(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = FactoryCreate(uniqueID, storage, informParentOfDirtinessDelegate);
            return itemToReturn;
        }

        /// <summary>
        /// Create a Lazinator item from bytes, where the Lazinator item is known to be a particular type, which may be a type that does not preserve unique ID information in serialized bytes. This method checks whether the type is such a type. If so, the unique ID is known, and the item is created. Otherwise, the unique ID is in the serialized bytes, and the item is instantiated based on that.
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="parent">The Lazinator parent of the item being created, or null if the item is at the top of the hierarchy or its parent is a struct</param>
        /// <returns>The deserialized Lazinator object</returns>
        public T FactoryCreate<T>(Type t, ReadOnlyMemory<byte> storage, ILazinator parent)
        {
            if (t.IsInterface)
                throw new LazinatorSerializationException("Impermissible attempt to create a Lazinator object based on ILazinator or a Lazinator interface instead of based on a concrete generic type. This may occur, for example, by attempting to use LazinatorList<ILazinator> or LazinatorList<IMyLazinator>, where IMyLazinator is a Lazinator exclusive interface. Instead, use LazinatorList<SomeLazinatorBaseType> or LazinatorList<INonexclusiveInterface>.");
            int? fixedUniqueID = GetFixedUniqueID(t);
            if (fixedUniqueID != null)
                return (T) FactoryCreate((int)fixedUniqueID, storage, parent);
            else
                return (T) FactoryCreate(storage, parent);
        }

        /// <summary>
        /// Create a Lazinator item from bytes and set a mechanism for informing its parent when the item has changed. This is generally used when the item is contained in a non-Lazinator collection, such as a .Net List.
        /// </summary>
        /// <param name="uniqueID">The type of the item to create</param>
        /// <param name="storage">The serialized bytes</param>
        /// <param name="informParentOfDirtinessDelegate">A delegate to be called when the item has changed.</param>
        /// <returns>The deserialized Lazinator object</returns>
        public ILazinator FactoryCreate(int uniqueID, ReadOnlyMemory<byte> serializedBytes, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (FactoriesByID.ContainsKey(uniqueID))
            {
                ILazinator selfSerialized = FactoriesByID[uniqueID]();
                InitializeDeserialized(selfSerialized, serializedBytes, informParentOfDirtinessDelegate);
                return selfSerialized;
            }
            else
                throw new UnknownSerializedTypeException(uniqueID);
        }

        private void InitializeDeserialized(ILazinator lazinatorType, ReadOnlyMemory<byte> serializedBytes, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            lazinatorType.DeserializationFactory = this;
            if (informParentOfDirtinessDelegate != null)
                lazinatorType.InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate;
            lazinatorType.LazinatorObjectBytes = serializedBytes;
        }

        /// <summary>
        /// Create from bytes a Lazinator object that does not need to inform a parent object if it has changed.
        /// </summary>
        /// <param name="storage">The serialized bytes</param>
        /// <returns>The deserialized Lazinator object</returns>
        public ILazinator FactoryCreate(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = FactoryCreate(uniqueID, storage);
            return itemToReturn;
        }

        /// <summary>
        /// Enumerate multiple objects stored successively in a single range of bytes and not in a Lazinator or .Net collection. Storing objects this way is useful when each is appended to the end of the storage.
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        public IEnumerable<ILazinator> FactoryCreateMultiple(ReadOnlyMemory<byte> storage)
        {
            while (true) // until "yield break"
            {
                if (storage.Length <= 1)
                    yield break;
                int bytesSoFar = 0;
                int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
                ILazinator itemToReturn = FactoryCreate(uniqueID, storage);
                int bytes = itemToReturn.LazinatorObjectBytes.Length;
                storage = storage.Slice(bytes);
                yield return itemToReturn;
            }
        }

        private ILazinator FactoryCreate(int uniqueID, ReadOnlyMemory<byte> serializedBytes)
        {
            if (FactoriesByID.ContainsKey(uniqueID))
            {
                ILazinator selfSerialized = FactoriesByID[uniqueID]();
                InitializeDeserialized(selfSerialized, serializedBytes);
                return selfSerialized;
            }
            else
                throw new UnknownSerializedTypeException(uniqueID);
        }

        private void InitializeDeserialized(ILazinator lazinatorType, ReadOnlyMemory<byte> serializedBytes)
        {
            lazinatorType.DeserializationFactory = this;
            lazinatorType.LazinatorObjectBytes = serializedBytes;
        }


        private void SetupAllTypesInAssemblies(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
                SetupAllTypesInAssembly(assembly);
        }

        private void SetupAllTypesInAssembly(Assembly assembly)
        {
            Type[] types = assembly.DefinedTypes
                .Where(x => x.ImplementedInterfaces.Contains(typeof(ILazinator)))
                .Where(x => !x.IsInterface && !x.IsAbstract)
                .Where(x => !x.ContainsGenericParameters)
                .Select(x => x.AsType())
                .ToArray();
            SetTypes(types);
        }

        private void SetTypes(Type[] typesImplementingILazinator)
        {
            typesImplementingILazinator = typesImplementingILazinator.Distinct().ToArray();
            foreach (Type type in typesImplementingILazinator)
            {
                var attribute = LazinatorReflection.GetLazinatorAttributeForILazinator(type);
                int uniqueID = attribute.UniqueID;
                if (FactoriesByID.ContainsKey(uniqueID))
                    throw new LazinatorDeserializationException($"The type {type} has self-serialization UniqueID of {uniqueID}, but that {uniqueID} is already used by {FactoriesByID[uniqueID]().GetType()}.");
                FactoriesByID[uniqueID] = GetFactoryForType(type);
                int? fixedUniqueID = GetFixedUniqueIDOrNull(type);
                if (fixedUniqueID != null)
                    FixedUniqueIDs[type] = (int)fixedUniqueID;
            }
        }

        public int? GetFixedUniqueID(Type type)
        {
            if (FixedUniqueIDs.ContainsKey(type))
                return FixedUniqueIDs[type];
            return null;
        }

        private Func<ILazinator> GetFactoryForType(Type type)
        {
            NewExpression newMyClass = System.Linq.Expressions.Expression.New(type);
            var converted = Expression.Convert(newMyClass, typeof(ILazinator));

            var lambda = Expression.Lambda<Func<ILazinator>>(converted);
            Func<ILazinator> compiledLambda = lambda.Compile();
            return compiledLambda;
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
                    .FirstOrDefault();

                    if (nonexclusiveAttribute == null)
                        fixedUniqueID = lazinatorAttribute.UniqueID;
                }
            }

            return fixedUniqueID;
        }
    }
}
