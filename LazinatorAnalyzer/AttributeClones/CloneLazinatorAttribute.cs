using System;

namespace LazinatorAnalyzer.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneLazinatorAttribute : Attribute
    {
        public bool Autogenerate { get; private set; }
        public int UniqueID { get; private set; }
        public int Version { get; private set; }

        public CloneLazinatorAttribute(int uniqueID)
        {
            UniqueID = uniqueID;
            Version = 0;
            Autogenerate = true;
        }

        public CloneLazinatorAttribute(int uniqueID, int version)
        {
            UniqueID = uniqueID;
            Version = version;
            Autogenerate = true;
        }

        public CloneLazinatorAttribute(int uniqueID, int version, bool autogenerate)
        {
            UniqueID = uniqueID;
            Version = version;
            Autogenerate = autogenerate;
        }
    }
}
