using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class ConcreteGeneric2 : AbstractGeneric1<int>, IConcreteGeneric2
    {
        public Example LazinatorExample { get; set; }
    }
}
