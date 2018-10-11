using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorTests.Examples.Structs;
using System;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleGrandchild)]
    public interface IExampleGrandchild : ILazinator
    {
        string MyString { get; set; }
        int MyInt { get; set; }
    }
}