using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Hierarchy
{
    [Lazinator((int)ExampleUniqueIDs.ReflexiveExample)]
    public interface IReflexiveExample : ILazinator
    {
        IReflexiveExample ReflexiveInterface { get; set; }
        ReflexiveExample ReflexiveClass { get; set; }
    }
}
