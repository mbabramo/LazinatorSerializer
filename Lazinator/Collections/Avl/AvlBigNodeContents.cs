using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public class AvlBigNodeContents<TKey, TValue> : IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        public long LeftCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public long SelfCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public long RightCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}