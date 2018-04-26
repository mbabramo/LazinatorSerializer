using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class NonexclusiveLazinatorAttribute : Attribute
    {
        public NonexclusiveLazinatorAttribute()
        {
        }
        
    }
}
