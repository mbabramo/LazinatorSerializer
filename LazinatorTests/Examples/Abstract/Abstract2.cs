using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public abstract partial class Abstract2 : Abstract1, IAbstract2
    {
        public abstract string String2 { get; set; }
    }
}
