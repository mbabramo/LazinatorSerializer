using System;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.NestedTuple)]
    public interface INestedTuple
    {
        Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> MyNestedTuple { get; set; }
    }
}