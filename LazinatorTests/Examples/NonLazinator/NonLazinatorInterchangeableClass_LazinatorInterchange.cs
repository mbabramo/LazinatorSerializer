using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonLazinator
{
    public class NonLazinatorInterchangeableClass_LazinatorInterchange : INonLazinatorInterchangeableClass_LazinatorInterchange
    {
        public string MyString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int MyInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
