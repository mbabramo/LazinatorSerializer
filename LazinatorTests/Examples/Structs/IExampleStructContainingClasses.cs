using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleStruct)]
    public interface IExampleStructContainingClasses
    {
        bool MyBool { get; set; }
        char MyChar { get; set; }
        ExampleChild MyChild1 { get; set; }
        ExampleChild MyChild2 { get; set; }
        List<int> MyListValues { get; set; }
        List<Example> MyLazinatorList { get; set; }
        bool MyLazinatorList_Dirty { get; set; }
        (NonLazinatorClass myitem1, int? myitem2) MyTuple { get; set; }
    }
}