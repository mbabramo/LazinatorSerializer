using System;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class NonexclusiveLazinatorAttribute : Attribute
    {
        public NonexclusiveLazinatorAttribute()
        {
        }
        
    }
}
