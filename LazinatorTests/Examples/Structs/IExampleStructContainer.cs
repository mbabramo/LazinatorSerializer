using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.ExampleStructContainer)]
    public interface IExampleStructContainer
    {
        ExampleStruct MyExampleStruct { get; set; }
        List<ExampleStruct> MyListExampleStruct { get; set; }
        List<WNullableStruct<ExampleStruct>> MyListNullableExampleStruct { get; set; }
        WInt IntWrapper { get; set; }
    }
}
