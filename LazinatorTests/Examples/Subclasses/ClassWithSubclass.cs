using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Subclasses
{
    public partial class ClassWithSubclass : IClassWithSubclass
    {
        public partial class SubclassWithinClass : ISubclassWithinClass
        {
        }
    }
}
