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
    public class DeserializationFactory
    {
        private Dictionary<int, Func<ILazinator>> FactoriesByID =
            new Dictionary<int, Func<ILazinator>>();

        public DeserializationFactory(Type[] types, bool loadAllEligibleTypesInCorrespondingAssemblies)
        {
            if (!loadAllEligibleTypesInCorrespondingAssemblies)
                SetTypes(types);
            else
                SetupAllTypesInAssemblies(types.Select(x => x.Assembly).Distinct().ToArray());
        }

        public DeserializationFactory(Assembly[] assemblies)
        {
            SetupAllTypesInAssemblies(assemblies);
        }

        public T Create<T>(int mostLikelyUniqueID, Func<T> funcToCreateMostLikely, ReadOnlyMemory<byte> storage, ILazinator parent) where T : ILazinator, new()
        {
            // Note: It's important that Create be generic, because that allows us to avoid boxing if the object being created is a struct and the uniqueID matches the mostLikelyUniqueID. 
            if (storage.Length <= 1)
                return default(T);
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

        public ILazinator FactoryCreate(ReadOnlyMemory<byte> storage, ILazinator parent)
        {
            if (storage.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = FactoryCreate(uniqueID, storage, parent);
            return itemToReturn;
        }

        public ILazinator FactoryCreate(int uniqueID, ReadOnlyMemory<byte> serializedBytes, ILazinator parent)
        {
            if (FactoriesByID.ContainsKey(uniqueID))
            {
                ILazinator selfSerialized = FactoriesByID[uniqueID]();
                InitializeDeserialized(selfSerialized, serializedBytes, parent);
                return selfSerialized;
            }
            else
                throw new UnknownSerializedTypeException(uniqueID);
        }

        public void InitializeDeserialized(ILazinator lazinatorType, ReadOnlyMemory<byte> serializedBytes, ILazinator parent)
        {
            lazinatorType.DeserializationFactory = this;
            lazinatorType.LazinatorParentClass = parent;
            lazinatorType.LazinatorObjectBytes = serializedBytes;
        }

        // The following overloads are identical without the parent and in some cases with an action to set dirtiness on the parent . Having separately overloads saves us the performance penalty of passing a null for parent.


        public T Create<T>(int mostLikelyUniqueID, Func<T> funcToCreateMostLikely, ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) where T : ILazinator, new()
        {
            // Note: It's important that Create be generic, because that allows us to avoid boxing if the object being created is a struct and the uniqueID matches the mostLikelyUniqueID. 
            if (storage.Length <= 1)
                return default(T);
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            T itemToReturn;
            if (uniqueID == mostLikelyUniqueID)
            {
                itemToReturn = funcToCreateMostLikely();
                InitializeDeserialized(itemToReturn, storage, informParentOfDirtinessDelegate);
            }
            else
                itemToReturn = (T)FactoryCreate(uniqueID, storage, informParentOfDirtinessDelegate); // we'll have to box structs here. We can't get around it because our dictionary's values are Func<ILazinator>, so we're going to have to cast that. We could alternatively have a dictionary of dictionaries indexed by type, but that might have a significant performance cost as well.
            return itemToReturn;
        }

        public ILazinator FactoryCreate(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = FactoryCreate(uniqueID, storage, informParentOfDirtinessDelegate);
            return itemToReturn;
        }

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

        public void InitializeDeserialized(ILazinator lazinatorType, ReadOnlyMemory<byte> serializedBytes, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            lazinatorType.DeserializationFactory = this;
            if (informParentOfDirtinessDelegate != null)
                lazinatorType.InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate;
            lazinatorType.LazinatorObjectBytes = serializedBytes;
        }

        public T Create<T>(int mostLikelyUniqueID, Func<T> funcToCreateMostLikely, ReadOnlyMemory<byte> storage) where T : ILazinator, new()
        {
            // Note: It's important that Create be generic, because that allows us to avoid boxing if the object being created is a struct and the uniqueID matches the mostLikelyUniqueID. 
            if (storage.Length <= 1)
                return default(T);
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            T itemToReturn;
            if (uniqueID == mostLikelyUniqueID)
            {
                itemToReturn = funcToCreateMostLikely();
                InitializeDeserialized(itemToReturn, storage);
            }
            else
                itemToReturn = (T)FactoryCreate(uniqueID, storage); // we'll have to box structs here. We can't get around it because our dictionary's values are Func<ILazinator>, so we're going to have to cast that. We could alternatively have a dictionary of dictionaries indexed by type, but that might have a significant performance cost as well.
            return itemToReturn;
        }

        public ILazinator FactoryCreate(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length <= 1)
                return null;
            int bytesSoFar = 0;
            int uniqueID = storage.Span.ToDecompressedInt(ref bytesSoFar);
            ILazinator itemToReturn = FactoryCreate(uniqueID, storage);
            return itemToReturn;
        }

        public ILazinator FactoryCreate(int uniqueID, ReadOnlyMemory<byte> serializedBytes)
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

        public void InitializeDeserialized(ILazinator lazinatorType, ReadOnlyMemory<byte> serializedBytes)
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
                    throw new LazinatorCodeGenException($"The type {type} has self-serialization UniqueID of {uniqueID}, but that {uniqueID} is already used by {FactoriesByID[uniqueID]().GetType()}.");
                FactoriesByID[uniqueID] = GetFactoryForType(type);
            }
        }

        private Func<ILazinator> GetFactoryForType(Type type)
        {
            NewExpression newMyClass = System.Linq.Expressions.Expression.New(type);
            var converted = Expression.Convert(newMyClass, typeof(ILazinator));

            var lambda = Expression.Lambda<Func<ILazinator>>(converted);
            Func<ILazinator> compiledLambda = lambda.Compile();
            return compiledLambda;
        }
    }
}
