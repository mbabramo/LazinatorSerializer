using System;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class LazinatorAttribute : Attribute
    {
        public bool Autogenerate { get; private set; }
        public int UniqueID { get; private set; }
        public int Version { get; private set; }

        public LazinatorAttribute(int uniqueID)
        {
            UniqueID = uniqueID;
            Version = 0;
            Autogenerate = true;
        }


        public LazinatorAttribute(int uniqueID, int version, bool autogenerate)
        {
            UniqueID = uniqueID;
            Version = version;
            Autogenerate = autogenerate;
        }
    }
}
