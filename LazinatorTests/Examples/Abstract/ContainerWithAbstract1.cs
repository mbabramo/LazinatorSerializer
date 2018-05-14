using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public class ContainerWithAbstract1 : IContainerWithAbstract1
    {
        public Abstract1 AbstractProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
