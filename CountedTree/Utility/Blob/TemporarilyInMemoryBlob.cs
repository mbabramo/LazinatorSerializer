using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace R8RUtilities
{
    public class TemporarilyInMemoryBlob<TKey> : IBlob<TKey>
    {
        public InMemoryBlob<TKey> InMemory;
        public IBlob<TKey> BackupStorage;
        public HashSet<TKey> ItemsChanged = new HashSet<TKey>();

        public TemporarilyInMemoryBlob(IBlob<TKey> backupStorage)
        {
            InMemory = new InMemoryBlob<TKey>();
            BackupStorage = backupStorage;
        }


        public bool SupportsEnumeration() => true;

        public Task<IEnumerable<UItem>> AsEnumerable<UItem>() where UItem : class => InMemory.AsEnumerable<UItem>();

        public async Task<object> GetBlob(TKey key)
        {
            object item = await InMemory.GetBlob(key);
            if (item == null)
                item = await BackupStorage.GetBlob(key);
            return item;
        }

        public async Task SetBlob(TKey key, object item)
        {
            ItemsChanged.Add(key);
            await InMemory.SetBlob(key, item);
        }

        public async Task<UItem> GetBlob<UItem>(TKey key)
        {
            UItem item = await InMemory.GetBlob<UItem>(key);
            if (item == null)
                item = await BackupStorage.GetBlob<UItem>(key);
            return item;
        }

        public async Task SetBlob<UItem>(TKey key, UItem item)
        {
            ItemsChanged.Add(key);
            await InMemory.SetBlob(key, item);
        }

        public async Task RemoveBlobFromCache(TKey key)
        {
            ItemsChanged.Remove(key);
            await InMemory.SetBlob(key, null);
        }

        public async Task UpdateBackupStorage()
        {
            var tasks = ItemsChanged
                .AsEnumerable()
                .Select(async key => BackupStorage.SetBlob(key, await InMemory.GetBlob(key)))
                .Select(x => x.Unwrap()) // so that we end up with Task, rather than Task<Task> as a result of the async lambda
                .ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
