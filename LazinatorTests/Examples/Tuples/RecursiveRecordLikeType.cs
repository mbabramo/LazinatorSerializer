﻿namespace LazinatorTests.Examples.Tuples
{
    public class RecursiveRecordLikeType
    {
        // This is not supported.

        public RecursiveRecordLikeType SameType { get; set; }
        public int MyInt { get; set; }

        public RecursiveRecordLikeType(RecursiveRecordLikeType sameType, int myInt)
        {
            SameType = sameType;
            MyInt = myInt;
        }
    }
}
