using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    public partial class NullableEnabledContext : INullableEnabledContext
    {
        public Example ExplicitlyNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Example NonNullableClass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IExample ExplicitlyNullableInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IExample NonNullableInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
