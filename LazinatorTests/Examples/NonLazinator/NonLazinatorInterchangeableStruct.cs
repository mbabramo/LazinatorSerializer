using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public readonly struct NonLazinatorInterchangeableStruct
    {
        public readonly string MyString;
        private readonly int MyInt;

        // note: this has no constructor with parameters myString and myInt. If it did, then it would be automatically handled.

        public NonLazinatorInterchangeableStruct(string aString, int anInt)
        {
            MyString = aString;
            MyInt = anInt;
        }
    }
}
