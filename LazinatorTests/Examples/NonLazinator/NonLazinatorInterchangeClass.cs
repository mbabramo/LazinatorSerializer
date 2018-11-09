using System.Reflection;

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorInterchangeClass : INonLazinatorInterchangeClass
    {
        static readonly FieldInfo _privateFieldInfoForMyInt = typeof(NonLazinatorInterchangeableClass).GetField("MyInt", BindingFlags.NonPublic | BindingFlags.Instance);

        public NonLazinatorInterchangeClass(
            NonLazinatorInterchangeableClass nonLazinatorInterchangeableClass) : this()
        {
            if (nonLazinatorInterchangeableClass == null)
                throw new System.Exception();
            MyString = nonLazinatorInterchangeableClass.MyString;
            MyInt = (int)_privateFieldInfoForMyInt.GetValue(nonLazinatorInterchangeableClass);
        }

        public NonLazinatorInterchangeableClass Interchange_NonLazinatorInterchangeableClass(bool cloning)
        {
            return new NonLazinatorInterchangeableClass(MyString, MyInt);
            // NOTE: If there weren't a constructor with the private field, we could do _privateFieldInfoForMyInt.SetValue(nonLazinatorInterchangeableClass, MyInt) 
        }
    }
}
