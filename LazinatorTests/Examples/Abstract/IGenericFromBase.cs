using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.GenericFromBase)]
    public interface IGenericFromBase<T> : IBase where T : ILazinator
    {
        T MyT { get; set; }
        int MyInt { get; set; }
    }
}