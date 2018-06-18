using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DerivedArray_Values)]
    public interface IDerivedArray_Values
    {
        int[] MyArrayInt_DerivedLevel { get; set; }
        bool MyArrayInt_DerivedLevel_Dirty { get; set; }
    }
}