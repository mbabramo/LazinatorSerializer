using System;

namespace LazinatorAnalyzer.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneNonexclusiveLazinatorAttribute : Attribute
    {
        public CloneNonexclusiveLazinatorAttribute()
        {
        }
        
    }
}
