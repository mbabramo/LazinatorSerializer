using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Wrappers;
using LazinatorCollections.Remote;

namespace LazinatorTests.Examples.RemoteHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.IRemoteLevel1)]
    public interface IRemoteLevel1 : ILazinator
    {
        int RemoteLevel1Int { get; set; }
        Remote<WGuid, RemoteLevel2> RemoteLevel2Item { get; set; }
    }
}