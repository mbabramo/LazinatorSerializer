using System;

namespace LazinatorCodeGen.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneSmallLazinatorAttribute : Attribute
    {
        public CloneSmallLazinatorAttribute()
        {
        }
        
    }
}