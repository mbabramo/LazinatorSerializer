using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    public partial class DerivedGenericContainer<T> : IDerivedGenericContainer<T> where T : ILazinator
    {
        public DerivedGenericContainer()
        {

        }
    }
}
