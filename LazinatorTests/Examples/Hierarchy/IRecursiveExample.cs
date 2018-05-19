using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Hierarchy
{
    [Lazinator((int)ExampleUniqueIDs.RecursiveExample)]
    public interface IRecursiveExample : ILazinator
    {
        IRecursiveExample RecursiveInterface { get; set; }
        RecursiveExample RecursiveClass { get; set; }
    }
}
