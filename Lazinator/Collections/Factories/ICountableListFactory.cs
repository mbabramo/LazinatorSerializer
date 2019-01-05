using Lazinator.Core;

namespace Lazinator.Collections.Factories
{
    public interface ICountableListFactory<TKey> where TKey : ILazinator
    {
        CountableListTypes ListType { get; set; }
    }
}