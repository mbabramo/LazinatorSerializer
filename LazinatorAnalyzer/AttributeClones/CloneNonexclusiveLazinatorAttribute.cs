using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneNonexclusiveLazinatorAttribute : Attribute
    {
        public CloneNonexclusiveLazinatorAttribute()
        {
        }
        
    }
}
