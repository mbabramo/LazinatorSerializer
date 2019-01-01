using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorTreeable)]
    public interface ILazinatorTreeable<T> where T : ILazinator, ILazinatorBranchable<T>
    {
        ILazinatorBranchable<T> GetRoot();
    }
}