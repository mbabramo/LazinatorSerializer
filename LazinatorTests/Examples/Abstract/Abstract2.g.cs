//72b30e93-e1ea-1dc2-61d2-8a36effda16a
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.27
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
{
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
    
    
    public partial class Abstract2 : ILazinator
    {
        protected int _IntList2_ByteIndex;
        protected virtual int _IntList2_ByteLength { get; }
        
        protected bool _String2_Accessed = false;
        public abstract string String2
        {
            get;
            set;
        }
        protected bool _IntList2_Accessed = false;
        public abstract System.Collections.Generic.List<int> IntList2
        {
            get;
            set;
        }
    }
}
