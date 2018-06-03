using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Collections
{
    public partial class DotNetList_Wrapper : IDotNetList_Wrapper
    {
        public List<WInt> MyListInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool MyListInt_Dirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
