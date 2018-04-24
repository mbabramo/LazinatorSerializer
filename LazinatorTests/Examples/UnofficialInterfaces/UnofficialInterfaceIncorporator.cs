using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public partial class UnofficialInterfaceIncorporator : IUnofficialInterfaceIncorporator
    {
        // MyInt has been made private because it's only in the unofficial interface (not directly in IUnofficialInterfaceIncorporator), and its accessibility has been set to private.
        // So it can be accessed in the partial class but not elsewhere. 
        public int MyIntMadePublic => MyInt;
    }
}
