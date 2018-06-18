using Lazinator.Core;

namespace LazinatorTests.Examples.Collections
{
    public partial class LazinatorListContainerGeneric<T> : ILazinatorListContainerGeneric<T> where T : ILazinator, new()
    {
        public LazinatorListContainerGeneric()
        {
        }
    }
}
