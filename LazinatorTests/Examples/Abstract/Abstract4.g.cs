//04dd18a1-4702-a5db-a411-30672aa8205a
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.25, on 5/15/2018 11:54:51 AM
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
    
    
    public partial class Abstract4 : Concrete3, ILazinator
    {
        internal int _IntList4_ByteIndex;
        internal virtual int _IntList4_ByteLength { get; }
        
        internal bool _String4_Accessed = false;
        public abstract string String4
        {
            get;
            set;
        }
        internal bool _IntList4_Accessed = false;
        public abstract System.Collections.Generic.List<int> IntList4
        {
            get;
            set;
        }
    }
}
