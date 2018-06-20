using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public partial class Simplifiable : ISimplifiable
    {
        public Example Example2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            MyIntsAre3 = MyInt == 3 && MyOtherInt == 3;
            ExampleHasDefaultValue = Example != null && Example.MyChar == 'X' && Example2 != null && Example.MyChar == 'X';
        }

        private void SetExampleToDefaultValue()
        {
            Example = new Example() { MyChar = 'X' };
            Example2 = new Example() { MyChar = 'X' };
        }

        private void SetMyIntsTo3()
        {
            MyInt = 3;
            MyOtherInt = 3;
        }
    }
}
