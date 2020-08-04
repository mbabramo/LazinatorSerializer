using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Specifies the first Lazinator version number of the class or struct that contains this attribute. It is necessary to use this attribute when
    /// adding a new property, if reading only Lazinator versions of the class or struct is to be supported.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IntroducedWithVersionAttribute : Attribute
    {
        public int Version;

        public IntroducedWithVersionAttribute(int version)
        {
            Version = version;
        }
    }
}