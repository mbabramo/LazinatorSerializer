using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that an item of this Lazinator class type will never have two Lazinator parents at the same time, i.e.
    /// will not simultaneously be in two different Lazinator hierarchies as a result of an assignment.
    /// If this attribute is used incorrectly, then only the most recent parent will be informed of changes to the item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HasSingleParentAttribute : Attribute
    {
    }
}
