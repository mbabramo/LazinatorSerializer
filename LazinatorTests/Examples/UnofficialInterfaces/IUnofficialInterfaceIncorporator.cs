using Lazinator.Attributes;
using LazinatorTests.Examples.Abstract;

namespace LazinatorTests.Examples
{
    [UnofficiallyIncorporateInterface("LazinatorTests.Examples.IUnofficialInterface", "private")]
    [Lazinator((int) ExampleUniqueIDs.UnofficialInterfaceIncorporator)]
    public interface IUnofficialInterfaceIncorporator
    {
        long MyLong { get; set; }
        Concrete5 MyConcrete5 { get; set; }
    }
}
