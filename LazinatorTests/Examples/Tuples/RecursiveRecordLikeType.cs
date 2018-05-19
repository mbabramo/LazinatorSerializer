using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Tuples
{
    public class RecursiveRecordLikeType
    {
        public RecursiveRecordLikeType SameType { get; set; }
        public int MyInt { get; set; }

        public RecursiveRecordLikeType(RecursiveRecordLikeType sameType, int myInt)
        {
            SameType = sameType;
            MyInt = myInt;
        }
    }
}
