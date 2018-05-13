using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public class InheritingClosedGeneric : IInheritingClosedGeneric
    {
        public int YetAnotherInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AnotherPropertyAdded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ExampleChild MyT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ExampleChild> MyListT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
