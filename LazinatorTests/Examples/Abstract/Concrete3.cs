using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class Concrete3 : Abstract2, IConcrete3
    {
        public string String3 { get; set; }
        public override string String2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string String1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
