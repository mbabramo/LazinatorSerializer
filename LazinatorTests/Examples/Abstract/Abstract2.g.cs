//c59e3f6f-f210-2223-379e-1e3b6f328a54
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.25
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lazinator.Buffers; 
using Lazinator.Collections;
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Abstract
{
    public partial class Abstract2 : ILazinator
    {
        internal int _IntList2_ByteIndex;
        internal virtual int _IntList2_ByteLength { get; }
        
        internal bool _String2_Accessed = false;
        public abstract string String2
        {
            get;
            set;
        }
        internal bool _IntList2_Accessed = false;
        public abstract System.Collections.Generic.List<int> IntList2
        {
            get;
            set;
        }
    }
}
