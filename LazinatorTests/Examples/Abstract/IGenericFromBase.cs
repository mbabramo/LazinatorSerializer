using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.GenericFromBase)]
    public interface IGenericFromBase<T> where T : ILazinator
    {
        T MyT { get; set; }
    }
}