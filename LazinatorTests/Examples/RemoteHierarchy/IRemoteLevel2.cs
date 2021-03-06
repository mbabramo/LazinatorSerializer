﻿using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.RemoteHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.RemoteLevel2)]
    public interface IRemoteLevel2 : ILazinator
    {
        int RemoteLevel2Int { get; set; }
    }
}