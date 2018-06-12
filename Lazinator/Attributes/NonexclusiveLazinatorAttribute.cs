using System;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class NonexclusiveLazinatorAttribute : Attribute
    {
        public int UniqueID { get; private set; }

        public NonexclusiveLazinatorAttribute(int uniqueID)
        {
            UniqueID = uniqueID;
        }

    }
}
