using Lazinator.Core;
using Lazinator.Attributes;
using System.Threading.Tasks;

namespace Lazinator.Collections.Remote
{
    /// <summary>
    /// A nongeneric interface to facilitate determining whether a particular Lazinator object is a remote object.
    /// </summary>
    public interface IRemote
    {
        Task SaveValue(bool freeRemoteStorage);
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.IRemote)]
    public interface IRemote<TKey, TValue> : IRemote
        where TKey : ILazinator
        where TValue : ILazinator
    {
        TKey Key { get; set; }
        bool StoreLocally { get; set; }
        TValue Local { get; set; }

        Task<TValue> GetValue();
    }
}