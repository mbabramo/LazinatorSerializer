using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.AbstractGeneric1)]
    [UnofficiallyIncorporateInterface("LazinatorTests.Examples.Abstract.IAbstractGeneric1Unofficial", "public")]
    public interface IAbstractGeneric1<T> : ILazinator
    {
        T MyT { get; set; }
        AbstractGeneric1<T>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric { get; set; }
        [FullyQualify]
        AbstractGeneric1<int>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2 { get; set; }
    }
}
