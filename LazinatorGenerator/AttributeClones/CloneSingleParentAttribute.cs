using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that an item of the corresponding class type will never share parenthood of any child property with another Lazinator, 
    /// i.e. the child will not simultaneously be in two different Lazinator hierarchies as a result of an assignment.
    /// If this attribute is used incorrectly, then only the most recent parent will be informed of changes to the child.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class CloneSingleParentAttribute : Attribute
    {
    }

}
