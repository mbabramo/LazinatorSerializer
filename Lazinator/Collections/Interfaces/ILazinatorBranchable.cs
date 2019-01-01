using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.LazinatorBranchable)]
    public interface ILazinatorBranchable<T> where T : ILazinator, ILazinatorBranchable<T>
    {
        short MaxBranches { get; }
        short CurrentBranches { get; }
        T GetBranchAt(short index);
        void SetBranchAt(short index, T branch);
    }
}
