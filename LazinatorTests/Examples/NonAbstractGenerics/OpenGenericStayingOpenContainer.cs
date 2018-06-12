using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class OpenGenericStayingOpenContainer : IOpenGenericStayingOpenContainer
    {
        public OpenGeneric<IExampleChild> ClosedGenericInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
