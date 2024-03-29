﻿using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Wrappers;
using Lazinator.Collections.Remote;

namespace LazinatorTests.Examples.RemoteHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.RemoteLevel1)]
    public interface IRemoteLevel1 : ILazinator
    {
        int RemoteLevel1Int { get; set; }
        Remote<WGuid, RemoteLevel2> RemoteLevel2Item { get; set; }
    }
}