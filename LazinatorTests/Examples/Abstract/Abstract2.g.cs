//590d17e3-e218-f2b4-34bd-546cad85ab33
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.401, on 2024/01/23 08:32:41.557 AM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
{
    #pragma warning disable 8019
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
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class Abstract2 : ILazinator
    {
        /* Property definitions */
        
        protected int _Example2_ByteIndex;
        protected int _IntList2_ByteIndex;
        protected virtual int _Example2_ByteLength => _IntList2_ByteIndex - _Example2_ByteIndex;
        protected virtual int _IntList2_ByteLength { get; }
        
        
        protected bool _String2_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract string String2
        {
            get;
            set;
        }
        
        protected bool _Example2_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract Example Example2
        {
            get;
            set;
        }
        
        protected bool _IntList2_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract List<Int32> IntList2
        {
            get;
            set;
        }
        
    }
}
#nullable restore
