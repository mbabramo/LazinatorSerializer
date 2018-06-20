using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public partial class Simplifiable : ISimplifiable
    {
        public void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            MyIntsAre3 = MyInt == 3 && MyOtherInt == 3;
            ExampleHasDefaultValue = Example != null && Example.MyChar == 'X';
        }

        private void SetExampleToDefaultValue()
        {
            Example = new Example() { MyChar = 'X' };
        }

        private void SetMyIntsTo3()
        {
            MyInt = 3;
            MyOtherInt = 3;
        }
    }
}
