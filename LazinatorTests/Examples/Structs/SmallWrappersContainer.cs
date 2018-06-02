using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Structs
{
    public partial class SmallWrappersContainer : ISmallWrappersContainer
    {
        public WByte WrappedByte { get; set; }
        public WByte WrappedByte2 { get; set; }
    }
}
