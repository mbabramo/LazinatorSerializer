using Lazinator.Attributes;

namespace LazinatorTests.Examples.NonLazinator
{
    [Lazinator((int)ExampleUniqueIDs.FromNonLazinatorBase)]
    interface IFromNonLazinatorBase
    {
        [DerivationKeyword("override")]
        int MyInt { get; set; }
    }
}
