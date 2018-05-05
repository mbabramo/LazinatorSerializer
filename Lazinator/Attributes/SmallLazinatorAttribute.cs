using System;

namespace Lazinator.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class SmallLazinatorAttribute : Attribute
    {
        public SmallLazinatorAttribute()
        {
        }
        
    }
}