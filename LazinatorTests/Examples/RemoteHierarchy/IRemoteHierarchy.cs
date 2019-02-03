using Lazinator.Core;
using Lazinator.Attributes;
using LazinatorCollections.Remote;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.RemoteHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.IRemoteHierarchy)]
    public interface IRemoteHierarchy
    {
        int TopOfHierarchyInt { get; set; }
        Remote<WGuid, RemoteLevel1> RemoteLevel1Item { get; set; }
    }
}