//87516c7b-365e-74dc-649d-4d4d610e7d4b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.60
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
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Abstract4 : Concrete3, ILazinator
    {
        protected int _IntList4_ByteIndex;
        protected virtual int _IntList4_ByteLength { get; }
        
        protected bool _String4_Accessed = false;
        public abstract string String4
        {
            get;
            set;
        }
        protected bool _IntList4_Accessed = false;
        public abstract List<int> IntList4
        {
            get;
            set;
        }
    }
}
