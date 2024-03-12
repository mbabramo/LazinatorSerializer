using System;
using System.Threading.Tasks;
using CountedTree.Node;
using CountedTree.NodeResults;
using CountedTree.Queries;
using Lazinator.Core;
using R8RUtilities;

namespace CountedTree.NodeStorage
{
    public interface INodeStorage
    {
        Task AddNode<TKey>(Guid treeID, CountedNode<TKey> node, IBlob<Guid> uintSetStorage) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>;
        Task<CountedNode<TKey>> GetNode<TKey>(Guid treeID, long nodeID) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>;
        Task<NodeResultBase<TKey>> ProcessQuery<TKey>(Guid treeID, long nodeID, NodeQueryBase<TKey> query) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>; 
        Task DeleteNode<TKey>(Guid treeID, long nodeID, IBlob<Guid> uintSetStorage) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>; 
    }
}
