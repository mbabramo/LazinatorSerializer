using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ExampleChild)]
    public interface IExampleChild
    {
        long MyLong { get; set; }
        short MyShort { get; set; }
    }
}