using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneEliminatedWithVersionAttribute : Attribute
    {
        public int Version;

        public CloneEliminatedWithVersionAttribute(int version)
        {
            Version = version;
        }
    }
}
