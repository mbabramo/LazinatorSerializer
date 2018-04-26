using System;

namespace LazinatorCodeGen.AttributeClones
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