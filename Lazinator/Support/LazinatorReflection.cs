using System;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Attributes;
using Lazinator.Exceptions;

namespace Lazinator.Support
{
    /// <summary>
    /// Utility methods to perform reflection on Lazinator types and return information about them.
    /// </summary>
    public static class LazinatorReflection
    {
        /// <summary>
        /// Gets the LazinatorAttribute for a type, or null if there is none.
        /// </summary>
        /// <param name="type">The Lazinator type</param>
        /// <returns></returns>
        public static LazinatorAttribute GetLazinatorAttributeForLazinatorMainType(Type type)
        {
            var correspondingInterface = GetCorrespondingExclusiveInterface(type);
            if (correspondingInterface == null)
                return null;
            var attribute = GetLazinatorAttributeForInterface(correspondingInterface);
            return attribute;
        }

        /// <summary>
        /// Get the Lazinator attribute for the only type that implements a Lazinator interface.
        /// </summary>
        /// <param name="correspondingInterface">The Lazinator interface type</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns whether the Lazinator interface type has NonbinaryHash set.
        /// </summary>
        /// <param name="correspondingInterface">The Lazinator interface type</param>
        /// <returns></returns>
        public static bool InterfaceHasUseNonbinaryHashAttribute(Type correspondingInterface)
        {
            return correspondingInterface.GetCustomAttributes(typeof(NonbinaryHashAttribute), false)
                .Select(x => (NonbinaryHashAttribute)x).Any();
        }

        /// <summary>
        /// Returns whether the Lazinator interface corresponding to the Lazinator type has NonbinaryHash set.
        /// </summary>
        /// <param name="mainType">The Lazinator type</param>
        /// <returns></returns>
        public static bool CorrespondingInterfaceHasUseNonbinaryHashAttribute(Type mainType)
        {
            var correspondingInterface = GetCorrespondingExclusiveInterface(mainType);
            if (correspondingInterface == null)
                return false;
            return correspondingInterface.GetCustomAttributes(typeof(NonbinaryHashAttribute), false)
                .Select(x => (NonbinaryHashAttribute)x).Any();
        }

        /// <summary>
        /// Returns the single nonexclusive Lazinator attribute for a Lazinator interface type (or null if none).
        /// </summary>
        /// <param name="interface">The Lazinator interface type</param>
        /// <returns></returns>
        public static NonexclusiveLazinatorAttribute GetNonexclusiveLazinatorAttributeForInterface(Type @interface)
        {
            var nonexclusiveAttributes = @interface.GetCustomAttributes(typeof(NonexclusiveLazinatorAttribute), false)
                .Select(x => (NonexclusiveLazinatorAttribute)x).ToList();
            NonexclusiveLazinatorAttribute attribute = nonexclusiveAttributes.FirstOrDefault();
            if (nonexclusiveAttributes.Skip(1).Any())
                throw new LazinatorDeserializationException(
                    $"The interface {@interface} should include at most one {nameof(NonexclusiveLazinatorAttribute)}, apart from attributes in derived classes.");
            return attribute;
        }

        /// <summary>
        /// Returns whether the type implements ILazinator.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ImplementsILazinator(Type type)
        {
            return type.GetInterface("ILazinator") != null;
        }

        /// <summary>
        /// Returns whether the type has a Lazinator attribute.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool HasLazinatorAttribute(Type interfaceType)
        {
            return interfaceType.GetCustomAttributes(typeof(LazinatorAttribute), false).Any();
        }

        /// <summary>
        /// Returns the base interface type that contains the Lazinator attribute
        /// </summary>
        /// <param name="interfaceType">The Lazinator interface type</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the base Lazinator type that implements ILazinator.
        /// </summary>
        /// <param name="objectType">The Lazinator type</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the interface corresponding to a Lazinator type
        /// </summary>
        /// <param name="type">The Lazinator type</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns all base types of a type.
        /// </summary>
        /// <param name="t">The type (which need not be a Lazinator type)</param>
        /// <returns></returns>
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