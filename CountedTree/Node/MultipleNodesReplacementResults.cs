using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace CountedTree.Node
{
    public partial class MultipleNodesReplacementResults<TKey> : IMultipleNodesReplacementResults<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
    }
}
