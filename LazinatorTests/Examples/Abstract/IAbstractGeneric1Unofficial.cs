using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGeneric1Unofficial)]
    interface IAbstractGeneric1Unofficial
    {
        int MyUnofficialInt { get; set; }
    }
}
