using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    public partial class GenericFromBase<T> : Base, IGenericFromBase<T> where T : ILazinator, new()
    {

    }
}
