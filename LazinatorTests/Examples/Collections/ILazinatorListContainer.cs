using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.ListContainer)]
    public interface ILazinatorListContainer
    {
        int MyInt { get; set; }
        LazinatorList<ExampleChild> MyList { get; set; }
        LazinatorList<WByte> MyStructList { get; set; }
        long MyLong { get; set; }
    }
}