using System;

namespace Lazinator.Attributes
{
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