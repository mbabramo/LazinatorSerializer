using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// An interface that Lazinator can directly serialize and that can be applied to many different lazinator classes. For example, you might have a nonexclusive lazinator interface IPolygon, with a property NumSides. Then, you might have Lazinator objects Triangle and Rectangle, implementing their own exclusive Lazinator interfaces, ITriangle and IRectangle, as well as IPolygon. Because IPolygon implements Lazinator, another Lazinator object (say, IBuilding) can include an IPolygon property.
    /// </summary>
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
