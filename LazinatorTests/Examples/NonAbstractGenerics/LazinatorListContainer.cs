using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public class LazinatorListContainer : ILazinatorListContainer
    {
        public LazinatorList<ExampleChild> MyList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
