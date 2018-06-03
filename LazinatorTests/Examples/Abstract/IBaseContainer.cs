using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.BaseContainer)]
    interface IBaseContainer
    {
        Base MyBase { get; set; }
    }
}