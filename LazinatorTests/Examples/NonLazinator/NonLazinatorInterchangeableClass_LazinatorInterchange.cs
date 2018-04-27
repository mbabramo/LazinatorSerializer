using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonLazinator
{
    public partial struct NonLazinatorInterchangeableClass_LazinatorInterchange : INonLazinatorInterchangeableClass_LazinatorInterchange
    {
        public NonLazinatorInterchangeableClass_LazinatorInterchange(
            NonLazinatorInterchangeableClass nonLazinatorInterchangeableClass) : this()
        {
            MyString = nonLazinatorInterchangeableClass.MyString;
            MyInt = nonLazinatorInterchangeableClass.MyInt;
        }

        public NonLazinatorInterchangeableClass Interchange()
        {
            return new NonLazinatorInterchangeableClass()
            {
                MyString = MyString,
                MyInt = MyInt
            };
        }
    }
}
