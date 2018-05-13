using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ClosedGeneric : IClosedGeneric
    {
        public ExampleChild MyT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ExampleChild> MyListT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
