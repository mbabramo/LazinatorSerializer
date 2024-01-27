using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that two items of this class type will never share any child property, i.e. the child
    /// will not simultaneously be in two different Lazinator hierarchies as a result of an assignment.
    /// If this attribute is used incorrectly, then only the most recent parent will be informed of changes to the item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CloneSingleParentAttribute : Attribute
    {
    }

}
