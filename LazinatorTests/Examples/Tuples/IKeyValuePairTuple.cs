using System;
using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.KeyValuePair)]
    public interface IKeyValuePairTuple
    {
        KeyValuePair<uint, ExampleChild> MyKeyValuePairSerialized { get; set; } 
    }
}