//64476f55-8721-ef1e-18a3-93ba54fd86cd
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.425, on 2024/03/13 01:51:51.339 PM.
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
    public partial class Abstract4 : Concrete3, ILazinator
    {
        /* Property definitions */
        
        protected int _IntList4_ByteIndex;
        protected virtual int _IntList4_ByteLength { get; }
        
        
        protected bool _String4_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract string String4
        {
            get;
            set;
        }
        
        protected bool _IntList4_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract List<Int32> IntList4
        {
            get;
            set;
        }
        
    }
}
#nullable restore
