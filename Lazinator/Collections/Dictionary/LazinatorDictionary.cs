using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Collections.Dictionary
{
    public partial class LazinatorDictionary<TKey, TValue> : ILazinatorDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        public LazinatorList<DictionaryBucket<TKey, TValue>> Buckets { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorList<TKey> Keys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorList<TValue> Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
