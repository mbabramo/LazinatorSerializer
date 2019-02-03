using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Remote
{
    public static class RemoteSaving
    {
        public async static Task SaveRemotes(ILazinator hierarchy, bool freeRemoteStorage = true, bool excludeTopOfHierarchy = false)
        {
            foreach (ILazinator remoteLazinator in hierarchy.EnumerateLazinatorNodes(x => x is IRemote && (!excludeTopOfHierarchy || x != hierarchy), true, x => true, true, false))
            {
                await SaveRemotes(remoteLazinator, true);
                IRemote r = (IRemote) remoteLazinator;
                await r.SaveValue(freeRemoteStorage);
            }
        }
    }
}
