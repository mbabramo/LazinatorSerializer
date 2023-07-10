using System;

namespace LazinatorGenerator.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneNonexclusiveLazinatorAttribute : Attribute
    {
        public CloneNonexclusiveLazinatorAttribute()
        {
        }
        
    }
}
