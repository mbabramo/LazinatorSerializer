using Lazinator.Attributes;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples
{
    [NonexclusiveLazinator]
    public interface IExampleNonexclusiveInterface : ILazinator
    {
        int MyInt { get; set; }
    }
}
