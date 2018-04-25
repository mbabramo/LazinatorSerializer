using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EliminatedWithVersionAttribute : Attribute
    {
        public int Version;

        public EliminatedWithVersionAttribute(int version)
        {
            Version = version;
        }
    }
}
