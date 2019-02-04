using System.Collections.Generic;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorCollections.Remote;
using LazinatorTests.Utilities;
using LazinatorTests.Examples.RemoteHierarchy;
using System.Threading.Tasks;

namespace LazinatorTests.Tests
{
    public class RemoteHierarchyTest
    {
        private InMemoryBlobStorage Storage = new InMemoryBlobStorage();

        private void SetupGuidBasedStorage()
        {
            RemoteManager<WGuid>.RemoteGetter = key => Storage.Get<WGuid>(key);
            RemoteManager<WGuid>.RemoteStoreLocally = r => false;
            RemoteManager<WGuid>.RemoteKeyGenerator = l => l.GetBinaryHashCode128();
            RemoteManager<WGuid>.RemoteSaver = (key, value) => Storage.Set<WGuid>(key, value);
        }

        [Fact]
        public async Task RemoteHierarchySerializeAndDeserialize()
        {
            RemoteHierarchy h = new RemoteHierarchy()
            {
                TopOfHierarchyInt = -1,
                RemoteLevel1Item = new Remote<WGuid, RemoteLevel1>(
                    new RemoteLevel1()
                    {
                        RemoteLevel1Int = 1,
                        RemoteLevel2Item = new Remote<WGuid, RemoteLevel2>(
                            new RemoteLevel2()
                            {
                                RemoteLevel2Int = 2,
                            }
                            )

                    }
                ),
            };
            await RemoteSaving.SaveBottomUp(h, true, false);
        }
    }
}
