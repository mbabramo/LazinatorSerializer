using Lazinator.Attributes;
using LazinatorTests.Examples.Abstract;

namespace LazinatorTests.Examples
{
    [UnofficiallyIncorporateInterface("LazinatorTests.Examples.IUnofficialInterface", "private")]
    [Lazinator((int) ExampleUniqueIDs.UnofficialInterfaceIncorporator)]
    public interface IUnofficialInterfaceIncorporator
    {
        Concrete5 MyOfficialObject { get; set; }
        long MyOfficialLong { get; set; }
    }
}
