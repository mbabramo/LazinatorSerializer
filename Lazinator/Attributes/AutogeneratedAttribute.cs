﻿using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that a partial class or partial struct has been been autogenerated by Lazinator, thus allowing Lazinator to distinguish its own code from user-generated code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class AutogeneratedAttribute : Attribute
    {
        public AutogeneratedAttribute()
        {
        }
    }
}