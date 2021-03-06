﻿using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleGrandchild)]
    public interface IExampleGrandchild : ILazinator
    {
        string AString { get; set; }
        int MyInt { get; set; }
    }
}