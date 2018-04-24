using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetStack_Values)]
    public interface IDotNetStack_Values
    {
        Stack<int> MyStackInt { get; set; }
        bool MyStackInt_Dirty { get; set; }
    }
}