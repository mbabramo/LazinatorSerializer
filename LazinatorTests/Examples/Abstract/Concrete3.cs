using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class Concrete3 : Abstract2, IConcrete3
    {
        public Concrete3() : base()
        {
            // We test this to make sure that setting a property in a constructor (before deserialization) will not trick Lazinator into thinking that the property has been accessed and thus does not need to be deserialized.
            Example3 = null;
        }
    }
}
