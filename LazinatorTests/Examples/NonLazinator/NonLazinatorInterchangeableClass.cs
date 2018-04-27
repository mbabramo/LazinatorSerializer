namespace LazinatorTests.Examples
{
    public class NonLazinatorInterchangeableClass
    {
        public string MyString;
        private int MyInt;

        // note: this has no constructor with parameters myString and myInt. If it did, then it would be automatically handled.

        public NonLazinatorInterchangeableClass(string aString, int anInt)
        {
            MyString = aString;
            MyInt = anInt;
        }
    }
}
