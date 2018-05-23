using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Subclasses
{
    public partial class ClassWithLocalEnum : IClassWithLocalEnum
    {
        public enum EnumWithinClass
        {
            Something,
            SomethingElse
        }
    }
}
