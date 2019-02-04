using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Remote
{
    public static class RemoteSaving
    {
        public async static Task SaveBottomUp(ILazinator hierarchy, bool freeRemoteStorage = true, bool excludeTopOfHierarchy = false)
        {
            hierarchy.UpdateStoredBuffer();
            foreach (ILazinator remoteLazinator in hierarchy.EnumerateLazinatorNodes(x => x is IRemote && (!excludeTopOfHierarchy || x != hierarchy), true, x => true, true, false))
            {
                await SaveBottomUp(remoteLazinator, true);
                IRemote r = (IRemote) remoteLazinator;
                await r.SaveValue(freeRemoteStorage);
            }
        }
    }
}
