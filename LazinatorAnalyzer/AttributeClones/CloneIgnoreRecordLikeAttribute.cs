using System;

namespace LazinatorCodeGen.AttributeClones
{
    /// <summary>
    /// Used to indicate that a class or struct should not be handled by automatically matching constructor parameters to properties. Thus, if the class does not implement ILazinator, the user must define a custom converter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class CloneIgnoreRecordLikeAttribute : Attribute
    {
        public CloneIgnoreRecordLikeAttribute()
        {
        }
    }
}