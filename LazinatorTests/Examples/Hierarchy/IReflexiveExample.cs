using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Hierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ReflexiveExample)]
    public interface IReflexiveExample
    {
        IReflexiveExample ReflexiveInterface { get; set; }
        ReflexiveExample ReflexiveClass { get; set; }
    }
}
