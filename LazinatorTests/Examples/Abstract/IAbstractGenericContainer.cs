using Lazinator.Attributes;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGenericContainer)]
    public interface IAbstractGenericContainer<T>
    {
        IAbstractGeneric1<T> Item { get; set; }
    }
}
