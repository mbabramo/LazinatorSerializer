using CountedTree.Node;
using Lazinator.Core;
using System;
using System.Threading.Tasks;

namespace CountedTree.Core
{

    public partial class TreeInfo : ITreeInfo
    {

        public TreeInfo(Guid treeID, long currentRootID, uint numValuesInTree, uint maxDepth, int workNeeded)
        {
            TreeID = treeID;
            CurrentRootID = currentRootID;
            NumValuesInTree = numValuesInTree;
            MaxDepth = maxDepth;
            WorkNeeded = workNeeded;
        }

        public async Task<CountedNode<TKey>> GetRoot<TKey>() where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            return await StorageFactory.GetNodeStorage().GetNode<TKey>(TreeID, CurrentRootID);
        }
    }
}
