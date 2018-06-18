using Lazinator.Core;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class OpenGeneric<T> : IOpenGeneric<T> where T : ILazinator
    {
    }
}
