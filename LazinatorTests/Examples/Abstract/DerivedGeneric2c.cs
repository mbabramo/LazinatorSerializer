using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    public partial class DerivedGeneric2c<T> : AbstractGeneric1<T>, IDerivedGeneric2c<T> where T : ILazinator
    {
        public DerivedGeneric2c()
        {
        }
    }
}
