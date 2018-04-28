using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Generics
{
    public partial class OpenGenericStayingOpenContainer : IOpenGenericStayingOpenContainer
    {
        public IOpenGenericStayingOpen<LazinatorWrapperFloat> ClosedGeneric { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
