using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneIntroducedWithVersionAttribute : Attribute
    {
        public int Version;

        public CloneIntroducedWithVersionAttribute(int version)
        {
            Version = version;
        }
    }
}