//b7899377-de38-b84b-8e32-7a5657796af0
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.319
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
        public abstract List<int> IntList2
        {
            get;
            set;
        }
        
    }
}
