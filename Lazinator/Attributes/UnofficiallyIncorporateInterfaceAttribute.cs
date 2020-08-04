using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Can be used to specify an interface that Lazinator will "unofficially" implement in the code behind, with the specified accessibility. This is useful where you want to include a property in your Lazinator object, but you don't want its accessibility to be public. Because all implementations of an interface must have public properties corresponding to the property in the interface, in that situation you wish to exclude the property from your Lazinator object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class UnofficiallyIncorporateInterfaceAttribute : Attribute
    {
        public string OtherInterfaceFullyQualifiedTypeName { get; private set; }
        public string Accessibility { get; private set; }

        /// <summary>
        /// Constructor for the UnofficiallyIncorporateInterfaceAttribute
        /// </summary>
        /// <param name="otherInterfaceFullyQualifiedTypeName">The fully qualified type name, including the namespace hierarchy and also an arity suffix for generic types (e.g., `1 or `2).</param>
        /// <param name="accessibility">The level of accessibility desired (e.g., private).</param>
        public UnofficiallyIncorporateInterfaceAttribute(string otherInterfaceFullyQualifiedTypeName, string accessibility)
        {
            OtherInterfaceFullyQualifiedTypeName = otherInterfaceFullyQualifiedTypeName;
            Accessibility = accessibility;
        }
    }
}
