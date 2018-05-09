using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public struct NonLazinatorStruct
    {
        public string MyString;
        public int MyInt;

        // The following constructor would ordinarily allow this to be handled automatically as a record-like type. But we include this in Lazinator.config in the list of IgnoreNonRecordLikeTypes.
        public NonLazinatorStruct(string myString, int myInt)
        {
            MyString = myString;
            MyInt = myInt;
        }
    }
}
