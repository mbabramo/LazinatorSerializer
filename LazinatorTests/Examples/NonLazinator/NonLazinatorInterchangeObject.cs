using System.Reflection;

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorInterchangeObject : INonLazinatorInterchangeObject
    {

        static readonly FieldInfo _privateFieldInfoForMyInt = typeof(NonLazinatorInterchangeableClass).GetField("MyInt", BindingFlags.NonPublic | BindingFlags.Instance);

        public NonLazinatorInterchangeObject(
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

        public NonLazinatorInterchangeableClass Interchange_NonLazinatorInterchangeableClass()
        {
            if (IsNull)
                return null;
            return new NonLazinatorInterchangeableClass(MyString, MyInt);
            // NOTE: If there weren't a constructor with the private field, we could do _privateFieldInfoForMyInt.SetValue(nonLazinatorInterchangeableClass, MyInt) 
        }
    }
}
