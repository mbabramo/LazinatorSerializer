//d0b1ba7c-c897-f91b-af1d-5b2f05b8b170
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.124
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Abstract2 : ILazinator
    {
        protected int _Example2_ByteIndex;
        protected int _IntList2_ByteIndex;
        protected virtual int _Example2_ByteLength => _IntList2_ByteIndex - _Example2_ByteIndex;
        protected virtual int _IntList2_ByteLength { get; }
        
        protected bool _String2_Accessed = false;
        public abstract string String2
        {
            get;
            set;
        }
        protected bool _Example2_Accessed = false;
        public abstract Example Example2
        {
            get;
            set;
        }
        protected bool _IntList2_Accessed = false;
        public abstract List<int> IntList2
        {
            get;
            set;
        }
        
    }
}
