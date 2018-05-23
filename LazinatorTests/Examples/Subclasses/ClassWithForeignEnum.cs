using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Subclasses
{
    public partial class ClassWithForeignEnum : IClassWithForeignEnum
    {
        public ClassWithLocalEnum.EnumWithinClass MyEnum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
