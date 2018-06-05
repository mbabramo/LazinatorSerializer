using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lazinator.Attributes;
using Lazinator.Exceptions;

namespace Lazinator.Support
{
    public static class LazinatorReflection
    {
        public static LazinatorAttribute GetLazinatorAttributeForILazinator(Type type)
        {
            var correspondingInterface = GetCorrespondingExclusiveInterface(type);
            if (correspondingInterface == null)
                return null;
            var attribute = GetLazinatorAttributeForInterface(correspondingInterface);
            return attribute;
        }

        public static LazinatorAttribute GetLazinatorAttributeForInterface(Type correspondingInterface)
        {
            var selfSerializeAttributes = correspondingInterface.GetCustomAttributes(typeof(LazinatorAttribute), false)
                .Select(x => (LazinatorAttribute) x).ToList();
            LazinatorAttribute attribute = selfSerializeAttributes.FirstOrDefault();
            if (selfSerializeAttributes.Skip(1).Any())
                throw new LazinatorDeserializationException(
                    $"The interface {correspondingInterface} should include only a single {nameof(LazinatorAttribute)}, apart from attributes in derived classes.");
            return attribute;
        }

        public static bool ImplementsILazinator(Type type)
        {
            return type.GetInterface("ILazinator") != null;
        }

        public static bool HasLazinatorAttribute(Type interfaceType)
        {
            return interfaceType.GetCustomAttributes(typeof(LazinatorAttribute), false).Any();
        }

        public static Type GetBaseInterfaceType(Type interfaceType)
        {
            if (interfaceType == null)
                return null;
            // an interface type itself may implement interfaces
            Type t = interfaceType.GetInterfaces().FirstOrDefault();
            if (t == null)
                return null;
            if (HasLazinatorAttribute(t))
                return t;
            else
                return GetBaseInterfaceType(t.BaseType);

        }

        public static Type GetBaseILazinatorType(Type objectType)
        {
            if (objectType == null)
                return null;
            Type t = objectType.BaseType;
            if (t == null || t.Equals(typeof(System.Object)) || t.Equals(typeof(System.ValueType)))
                return null;
            if (ImplementsILazinator(t))
                return t;
            else
                return GetBaseILazinatorType(t.BaseType);
        }

        public static Type GetCorrespondingExclusiveInterface(Type type)
        {
            var allInterfaces = type.GetInterfaces();
            var minimalInterfaces = allInterfaces
                .Except(allInterfaces.SelectMany(t => t.GetInterfaces()))
                .Except(BaseTypes(type).SelectMany(t => t.GetInterfaces()))
                .Where(x => x.GetCustomAttributes(typeof(LazinatorAttribute), false).Any())
                .ToList();
            int count = minimalInterfaces.Count();
            if (count == 0)
                return null;
            if (count > 1)
                    throw new LazinatorDeserializationException(
                    $"An ILazinator type must not directly implement more than one interface with a Lazinator attribute. The type {type} directly implements {minimalInterfaces.Count()} interfaces ({String.Join(",", minimalInterfaces.Select(x => x.Name).ToArray())}) with a Lazinator attribute.");

            var correspondingInterface = minimalInterfaces.Single();
            return correspondingInterface;
        }

        private static IEnumerable<Type> BaseTypes(Type t)
        {
            Type currentType = t.BaseType;
            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }
        
    }
}