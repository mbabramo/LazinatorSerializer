using Lazinator.Attributes;
using LazinatorTests.Examples.Abstract;

namespace LazinatorTests.Examples
{
    // no lazinator attribute here, since it's unofficial

    [Lazinator((int)ExampleUniqueIDs.UnofficialInterface)]
    public interface IUnofficialInterface
    {
        int MyInt {get; set;}
        Concrete3 MyConcrete3 { get; set; }
    }
}
