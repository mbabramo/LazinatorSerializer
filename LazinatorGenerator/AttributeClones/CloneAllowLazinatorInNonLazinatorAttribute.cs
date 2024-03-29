﻿using System;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Indicates that a Lazinator object can serve as a generic type argument to a non-Lazinator collection, even if ordinarily prohibited as a result of a configuration setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneAllowLazinatorInNonLazinatorAttribute : Attribute
    {
        public CloneAllowLazinatorInNonLazinatorAttribute()
        {
        }
    }
}
