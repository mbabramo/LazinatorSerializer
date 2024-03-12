using Lazinator.Core;
using R8RUtilities;
using System;
using System.Threading.Tasks;

namespace CountedTree.Updating
{
    public class TreeHistoryManagerAccess : ITreeHistoryManagerAccess
    {
        IBlob<Guid> BlobStorage;

        public TreeHistoryManagerAccess(IBlob<Guid> blobStorage)
        {
            BlobStorage = blobStorage;
        }

        public Task<ITreeHistoryManager<TKey>> Get<TKey>(Guid key) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            return BlobStorage.GetBlob<ITreeHistoryManager<TKey>>(key);
        }

        public async Task Set<TKey>(Guid key, ITreeHistoryManager<TKey> item) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            await BlobStorage.SetBlob<ITreeHistoryManager<TKey>>(key, item);
        }
    }
}
