using LazinatorTests.AnotherNamespace;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    public partial class ContainerForStructInAnotherNamespace : IContainerForStructInAnotherNamespace
    {
        public StructInAnotherNamespace MyStruct { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public StructInAnotherNamespace? MyNullableStruct { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
