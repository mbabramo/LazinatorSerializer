using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Collections.Remote
{
    /// <summary>
    /// A utility method for saving all items in a hierarchy that must be saved remotely.
    /// </summary>
    public static class RemoteSaving
    {
        public async static Task SaveRemotes(ILazinator hierarchy, bool freeRemoteStorage = true, bool excludeTopOfHierarchy = false)
        {
            hierarchy.SerializeLazinator();
            foreach (ILazinator remoteLazinator in hierarchy.EnumerateLazinatorNodes(x => x is IRemote && (!excludeTopOfHierarchy || x != hierarchy), true, x => true, true, false))
            {
                await SaveRemotes(remoteLazinator, freeRemoteStorage, true);
                IRemote r = (IRemote) remoteLazinator;
                await r.SaveValue(freeRemoteStorage);
            }
        }
    }
}
