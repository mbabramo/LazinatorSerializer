//9388cf9a-a9ea-ec57-1d03-b515928e4135
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.397
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Abstract2 : ILazinator
    {
        /* Property definitions */
        
        protected int _Example2_ByteIndex;
        protected int _IntList2_ByteIndex;
        protected virtual int _Example2_ByteLength => _IntList2_ByteIndex - _Example2_ByteIndex;
        protected virtual int _IntList2_ByteLength { get; }
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _String2_Accessed = false;
        public abstract string String2
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Example2_Accessed = false;
        public abstract Example Example2
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IntList2_Accessed = false;
        public abstract List<Int32> IntList2
        {
            get;
            set;
        }
        
    }
}
