using Lazinator.Core;
using System;

namespace CountedTree.TypeAhead
{
    public partial class MoreResultsRequest<R> : IMoreResultsRequest<R> where R : ILazinator,
              IComparable,
              IComparable<R>,
              IEquatable<R>
    {
    }
}
