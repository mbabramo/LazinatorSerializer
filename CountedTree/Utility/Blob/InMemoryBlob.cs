using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace R8RUtilities
{
    public class InMemoryBlob<TKey> : IBlob<TKey>
    {
        Dictionary<TKey, object> Storage = new Dictionary<TKey, object>();

        public bool SupportsEnumeration() => true;

        public Task<IEnumerable<UItem>> AsEnumerable<UItem>() where UItem : class => Task.FromResult(Storage.AsEnumerable().Select(x => x.Value as UItem));

        public IEnumerable<TKey> Keys()
        {
            return Storage.AsEnumerable().Select(x => x.Key);
        }

        public IEnumerable<object> AsEnumerable()
        {
            return Storage.AsEnumerable().Select(x => x.Value);
        }

        public Task<object> GetBlob(TKey key)
        {
            if (Storage.ContainsKey(key))
                return Task.FromResult(Storage[key]);
            return Task.FromResult<object>(null);
        }

        public async Task<UItem> GetBlob<UItem>(TKey key)
        {
            return (UItem) await GetBlob(key);
        }

        public Task SetBlob(TKey key, object item)
        {
            if (item == null)
            {
                if (Storage.ContainsKey(key))
                    Storage.Remove(key);
            }
            else
                Storage[key] = item;
            return Task.CompletedTask;
        }

        public async Task SetBlob<UItem>(TKey key, UItem item)
        {
            await SetBlob(key, (object) item);
        }
    }
}
