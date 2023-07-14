using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Identifies a class as a Lazinator class, supporting serialization and deserialization. The attribute
    /// indicates that code-behind must be created for the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class LazinatorAttribute : Attribute
    {
        public bool Autogenerate { get; private set; }
        public int UniqueID { get; private set; }
        /// <summary>
        /// A version number. Set this to a negative number to indicate that the UniqueID should not be persisted. 
        /// </summary>
        public int Version { get; private set; }

        public LazinatorAttribute(int uniqueID)
        {
            UniqueID = uniqueID;
            Version = 0;
            Autogenerate = true;
        }

        public LazinatorAttribute(int uniqueID, int version)
        {
            UniqueID = uniqueID;
            Version = version;
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
