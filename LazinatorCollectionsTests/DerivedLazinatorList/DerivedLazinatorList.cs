using Lazinator.Core;
using LazinatorCollections;

namespace LazinatorCollectionsTests
{
    public partial class DerivedLazinatorList<T> : LazinatorList<T>, IDerivedLazinatorList<T> where T : ILazinator
    {
    }
}
