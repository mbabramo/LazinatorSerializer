using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeContents<TKey, TValue> : IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
    }
}