using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public partial class Simplifiable : ISimplifiable
    {
        public const string LongString = "The quick brown fox jumps over the lazy dog.";

        public void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            MyIntsAre3 = MyInt == 3 && MyOtherInt == 3;
            ExampleHasDefaultValue = Example != null && Example.MyChar == 'X' && Example.MyString == LongString;
            Example2Char = Example2?.MyChar;
            Example3IsNull = Example3 == null;
        }

        private void SetExampleToDefaultValue()
        {
            Example = new Example() { MyChar = 'X', MyString = LongString };
        }

        private void SetMyIntsTo3()
        {
            MyInt = 3;
            MyOtherInt = 3;
        }
    }
}
