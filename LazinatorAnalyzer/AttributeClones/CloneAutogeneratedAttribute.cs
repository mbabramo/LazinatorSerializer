﻿using System;

namespace LazinatorAnalyzer.AttributeClones
{
    /// <summary>
    /// Used to indicate that a partial class or struct has been been autogenerated by Lazinator, thus allowing Lazinator to distinguish its own code from user-generated code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class CloneAutogeneratedAttribute : Attribute
    {
        public CloneAutogeneratedAttribute()
        {
        }
    }
}