using Lazinator.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountedTree.UintSets
{
    public interface IUintSetAccess
    {
        Task Initialize();
        Task InitializeWithLoc(bool noMoreThan16ChildrenPerNode);
        Task Initialize(UintSet uintSet);
        Task InitializeWithLoc(UintSetWithLoc uintSetWithLoc);
        Task<UintSet> GetUintSet();
        Task<UintSetWithLoc> GetUintSetWithLoc();
        Task Change(bool alwaysIntegrateIntoMain, List<WUInt32> indicesToRemove, List<WUInt32> indicesToAdd, List<byte> locationsToAdd);
        Task Delete();
    }
}
