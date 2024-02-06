using System.Collections.Generic;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.NonAbstractGenerics;
using Lazinator.Collections.Remote;
using LazinatorTests.Utilities;
using LazinatorTests.Examples.RemoteHierarchy;
using System.Threading.Tasks;
using System;
using Lazinator.Persistence;

namespace LazinatorTests.Tests
{
    public class RemoteHierarchyTest
    {
        private InMemoryBlobManager Storage = new InMemoryBlobManager();

        private void ClearAllSetup()
        {
            ClearSetup<RemoteLevel1>();
            ClearSetup<RemoteLevel2>();
        }

        private void ClearSetup<TValue>() where TValue : class, ILazinator
        {
            RemoteManager<WGuid>.RemoteGetter = null;
            RemoteManager<WGuid>.RemoteStoreLocally = null;
            RemoteManager<WGuid>.RemoteKeyGenerator = null;
            RemoteManager<WGuid>.RemoteSaver = null;
            RemoteManager<WGuid, TValue>.RemoteGetter = null;
            RemoteManager<WGuid, TValue>.RemoteStoreLocally = null;
            RemoteManager<WGuid, TValue>.RemoteKeyGenerator = null;
            RemoteManager<WGuid, TValue>.RemoteSaver = null;
        }

        private void SetupAllKeyValueStorage()
        {
            SetupKeyValueStorage<RemoteLevel1>();
            SetupKeyValueStorage<RemoteLevel2>();
        }

        private void SetupKeyValueStorage<TValue>() where TValue : class, ILazinator
        {
            RemoteManager<WGuid, TValue>.RemoteGetter = key => Storage.Get<WGuid, TValue>(key);
            RemoteManager<WGuid, TValue>.RemoteStoreLocally = r => false;
            RemoteManager<WGuid, TValue>.RemoteKeyGenerator = l => l.GetBinaryHashCode128();
            RemoteManager<WGuid, TValue>.RemoteSaver = (key, value) => Storage.Set<WGuid, TValue>(key, value);
        }

        private void SetupKeyOnlyStorage()
        {
            RemoteManager<WGuid>.RemoteGetter = key => Storage.Get<WGuid>(key);
            RemoteManager<WGuid>.RemoteStoreLocally = r => false;
            RemoteManager<WGuid>.RemoteKeyGenerator = l => l.GetBinaryHashCode128();
            RemoteManager<WGuid>.RemoteSaver = (key, value) => Storage.Set<WGuid>(key, value);
        }

        private static async Task VerifyRemoteHierarchySaving(bool storeLocally)
        {
            RemoteHierarchy h = GetRemoteHierarchy();
            await RemoteSaving.SaveRemotes(h, true, false);
            await VerifyCanLoadRemoteItems(h, storeLocally);
            var h2 = h.CloneLazinatorTyped();
            await VerifyCanLoadRemoteItems(h2, storeLocally);
        }

        [Fact]
        public async Task RemoteHierarchyWithRemoteStorage_KeyOnlySetup()
        {
            ClearAllSetup();
            SetupKeyOnlyStorage();
            await VerifyRemoteHierarchySaving(false);
        }

        [Fact]
        public async Task RemoteHierarchyWithRemoteStorage_KeyValueSetup()
        {
            ClearAllSetup();
            SetupAllKeyValueStorage();
            await VerifyRemoteHierarchySaving(false);
        }

        [Fact]
        public async Task RemoteHierarchyWithLocalStorage()
        {
            ClearAllSetup();
            await VerifyRemoteHierarchySaving(true);
        }

        private static RemoteHierarchy GetRemoteHierarchy()
        {
            return new RemoteHierarchy()
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
        }

        private static async Task VerifyCanLoadRemoteItems(RemoteHierarchy h, bool remoteItemsStoredLocally)
        {
            h.TopOfHierarchyInt.Should().Be(-1);
            h.RemoteLevel1Item.StoreLocally.Should().Be(remoteItemsStoredLocally);
            var level1Item = await h.RemoteLevel1Item.GetValue();
            level1Item.RemoteLevel1Int.Should().Be(1);
            level1Item.RemoteLevel2Item.StoreLocally.Should().Be(remoteItemsStoredLocally);
            var level2Item = await level1Item.RemoteLevel2Item.GetValue();
            level2Item.RemoteLevel2Int.Should().Be(2);
        }
    }
}
