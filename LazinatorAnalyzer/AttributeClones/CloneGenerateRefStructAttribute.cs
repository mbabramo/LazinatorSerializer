using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Used to indicate that, within the Lazinator class/struct, a child ref struct should be generated with the same basic functionality as the parent Lazinator object. Also, ToRefStruct and FromRefStruct methods will be generated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class CloneGenerateRefStructAttribute : Attribute
    {
        public CloneGenerateRefStructAttribute()
        {
        }
    }
}
