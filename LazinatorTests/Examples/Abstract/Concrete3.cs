﻿namespace LazinatorTests.Examples.Abstract
{
    public partial class Concrete3 : Abstract2, IConcrete3
    {
        public Concrete3() : base()
        {
            // We test this to make sure that setting a property in a constructor of the containing class (before deserialization) will not trick Lazinator into thinking that the property has been accessed and thus does not need to be deserialized.
            Example2 = null;
            Example3 = null;
        }
    }
}
