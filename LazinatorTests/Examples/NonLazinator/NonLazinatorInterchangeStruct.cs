using System.Reflection;

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorInterchangeStruct : INonLazinatorInterchangeStruct
    {
        static readonly FieldInfo _privateFieldInfoForMyInt = typeof(NonLazinatorInterchangeableStruct).GetField("MyInt", BindingFlags.NonPublic | BindingFlags.Instance);

        public NonLazinatorInterchangeStruct(
            NonLazinatorInterchangeableStruct nonLazinatorInterchangeableStruct) : this()
        {
            if (System.Collections.Generic.EqualityComparer<NonLazinatorInterchangeableStruct>.Default.Equals(nonLazinatorInterchangeableStruct, default(NonLazinatorInterchangeableStruct)))
                throw new System.Exception();
            MyString = nonLazinatorInterchangeableStruct.MyString;
            MyInt = (int)_privateFieldInfoForMyInt.GetValue(nonLazinatorInterchangeableStruct);
        }

        public NonLazinatorInterchangeableStruct Interchange_NonLazinatorInterchangeableStruct()
        {
            return new NonLazinatorInterchangeableStruct(MyString, MyInt);
            // NOTE: If there weren't a constructor with the private field, we could do _privateFieldInfoForMyInt.SetValue(nonLazinatorInterchangeableClass, MyInt) 
        }
    }
}
