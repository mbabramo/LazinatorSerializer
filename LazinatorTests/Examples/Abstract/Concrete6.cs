using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class Concrete6 : Concrete5, IConcrete6
    {
        // create a constructor to make sure that public parameterless constructor is automatically created
        public Concrete6(string string1, string string2, string string3, string string4, string string5)
        {
            String1 = string1;
            String2 = string2;
            String3 = string3;
            String4 = string4;
            String5 = string5;
        }
    }
}
