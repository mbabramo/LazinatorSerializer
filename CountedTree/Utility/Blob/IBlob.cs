using System.Collections.Generic;
using System.Threading.Tasks;

namespace R8RUtilities
{
    public interface IBlob<TKey>
    {
        Task<UItem> GetBlob<UItem>(TKey key);
        Task SetBlob<UItem>(TKey key, UItem item);
        Task<object> GetBlob(TKey key);
        Task SetBlob(TKey key, object item);
        bool SupportsEnumeration();
        Task<IEnumerable<UItem>> AsEnumerable<UItem>() where UItem : class;
    }
}
