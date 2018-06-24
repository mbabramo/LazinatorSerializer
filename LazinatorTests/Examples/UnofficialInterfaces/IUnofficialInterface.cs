using Lazinator.Attributes;
using LazinatorTests.Examples.Abstract;

namespace LazinatorTests.Examples
{
    // no lazinator attribute here, since it's unofficial

    [Lazinator((int)ExampleUniqueIDs.UnofficialInterface)]
    public interface IUnofficialInterface
    {
        int MyUnofficialInt {get; set;}
        Concrete3 MyUnofficialObject { get; set; }
    }
}
