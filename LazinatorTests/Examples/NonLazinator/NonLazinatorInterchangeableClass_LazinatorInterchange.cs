using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LazinatorTests.Examples.NonLazinator
{
    public partial struct NonLazinatorInterchangeableClass_LazinatorInterchange : INonLazinatorInterchangeableClass_LazinatorInterchange
    {

        static readonly FieldInfo _privateFieldInfoForMyInt = typeof(NonLazinatorInterchangeableClass).GetField("MyInt", BindingFlags.NonPublic | BindingFlags.Instance);

        public NonLazinatorInterchangeableClass_LazinatorInterchange(
            NonLazinatorInterchangeableClass nonLazinatorInterchangeableClass) : this()
        {
            if (nonLazinatorInterchangeableClass == null)
                IsNull = true;
            else
            {
                MyString = nonLazinatorInterchangeableClass.MyString;
                MyInt = (int) _privateFieldInfoForMyInt.GetValue(nonLazinatorInterchangeableClass);
            }
        }

        public NonLazinatorInterchangeableClass Interchange()
        {
            if (IsNull)
                return null;
            return new NonLazinatorInterchangeableClass(MyString, MyInt);
            // NOTE: If there weren't a constructor with the private field, we could do _privateFieldInfoForMyInt.SetValue(nonLazinatorInterchangeableClass, MyInt) 
        }
    }
}
