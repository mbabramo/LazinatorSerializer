﻿using System;

namespace LazinatorAnalyzer.AttributeClones
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CloneSmallLazinatorAttribute : Attribute
    {
        public CloneSmallLazinatorAttribute()
        {
        }
        
    }
}