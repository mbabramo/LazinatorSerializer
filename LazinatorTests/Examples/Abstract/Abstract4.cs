using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public abstract partial class Abstract4 : Concrete3, IAbstract4
    {
        public string String4 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
