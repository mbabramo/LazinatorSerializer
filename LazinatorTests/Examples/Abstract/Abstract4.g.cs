//5292140f-3f0a-765e-c2d4-aa02aed9b261
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.391
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
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
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
