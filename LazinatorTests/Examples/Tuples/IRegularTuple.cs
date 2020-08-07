using System;
using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.RegularTuple)]
    public interface IRegularTuple
    {
        Tuple<uint, ExampleChild, NonLazinatorClass> MyTupleSerialized { get; set; }
        Tuple<uint, ExampleChild, NonLazinatorClass> MyTupleSerialized2 { get; set; }
        Tuple<uint?, ExampleChild, NonLazinatorClass> MyTupleSerialized3 { get; set; }
        Tuple<int, ExampleStructContainingClasses> MyTupleSerialized4 { get; set; }
        Tuple<int, ExampleStructContainingClasses?> MyTupleSerialized5 { get; set; }
        List<Tuple<uint, ExampleChild, NonLazinatorClass>> MyListTuple { get; set; }
    }
}