using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that two items of this class type will never share any child property, i.e. the child
    /// will not simultaneously be in two different Lazinator hierarchies as a result of an assignment.
    /// If this attribute is used incorrectly, then only the most recent parent will be informed of changes to the child.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SingleParentAttribute : Attribute
    {
    }
}
