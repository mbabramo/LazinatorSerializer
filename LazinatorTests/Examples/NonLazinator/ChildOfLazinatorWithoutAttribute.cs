namespace LazinatorTests.Examples.NonLazinator
{
    // This class descends from a Lazinator class, but it has no Lazinator attribute of its own. We should be able to serialize and deserialize the underlying class, though no methods are added.
    public partial class ChildOfLazinatorWithoutAttribute : FromNonLazinatorBase
    {
    }
}
