using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.StructTuple)]
    public interface IStructTuple
    {
        (uint, ExampleChild, NonLazinatorClass) MyValueTupleSerialized { get; set; }
        (int first, double second)? MyNullableTuple { get; set; }
    }
}