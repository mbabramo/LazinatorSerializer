using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Core;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.DerivedLazinatorList)]
    interface IDerivedLazinatorList<T> : ILazinatorList<T> where T : ILazinator
    {
        string MyListName { get; set; }
    }
}
