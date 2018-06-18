using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    // no lazinator attribute here, since it's unofficial

    [Lazinator((int)ExampleUniqueIDs.UnofficialInterface)]
    public interface IUnofficialInterface
    {
        int MyInt {get; set;}
    }
}
