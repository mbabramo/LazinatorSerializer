using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples
{
    [NonexclusiveLazinator((int)ExampleUniqueIDs.NonexclusiveLazinatorAttribute)]
    public interface IExampleNonexclusiveInterface : ILazinator
    {
        int MyInt { get; set; }
    }
}
