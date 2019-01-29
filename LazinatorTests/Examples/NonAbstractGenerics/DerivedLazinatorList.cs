using LazinatorCollections;
using Lazinator.Core;

namespace LazinatorTests.Examples
{
    public partial class DerivedLazinatorList<T> : LazinatorList<T>, IDerivedLazinatorList<T> where T : ILazinator
    {
        public DerivedLazinatorList()
        {
        }
    }
}
