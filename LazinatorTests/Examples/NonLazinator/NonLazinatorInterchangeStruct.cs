using System.Reflection;

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorInterchangeStruct : INonLazinatorInterchangeStruct
    {
        static readonly FieldInfo _privateFieldInfoForMyInt = typeof(NonLazinatorInterchangeableStruct).GetField("MyInt", BindingFlags.NonPublic | BindingFlags.Instance);

        public NonLazinatorInterchangeStruct(
            NonLazinatorInterchangeableStruct nonLazinatorInterchangeableStruct) : this()
        {
            MyString = nonLazinatorInterchangeableStruct.MyString;
            MyInt = (int)_privateFieldInfoForMyInt.GetValue(nonLazinatorInterchangeableStruct);
        }

        public NonLazinatorInterchangeableStruct Interchange_NonLazinatorInterchangeableStruct(bool cloning)
        {
            return new NonLazinatorInterchangeableStruct(MyString, MyInt);
            // NOTE: If there weren't a constructor with the private field, we could do _privateFieldInfoForMyInt.SetValue(nonLazinatorInterchangeableClass, MyInt) 
        }
    }
}
