using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NonLazinatorInterchangeableClass_LazinatorInterchange)]
    public interface INonLazinatorInterchangeableClass_LazinatorInterchange
    {
        bool IsNull { get; set; }
        string MyString { get; set; }
        int MyInt { get; set; }
    }
}
