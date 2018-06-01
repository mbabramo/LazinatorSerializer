using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Collections
{
    public partial class DerivedArray_Values : Array_Values, IDerivedArray_Values
    { 
        public int[] MyArrayInt_DerivedLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool MyArrayInt_DerivedLevel_Dirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
