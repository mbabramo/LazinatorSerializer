using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.ExampleStructContainingStruct)]
    public interface IExampleStructContainingStruct
    {
        ExampleStruct MyExampleStruct { get; set; }
        // ExampleStruct? MyExampleNullableStruct { get; set; }
    }
}
