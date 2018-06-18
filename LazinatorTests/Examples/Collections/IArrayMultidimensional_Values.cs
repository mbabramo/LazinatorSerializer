using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.ArrayMultidimensionalAndJagged)]
    public interface IArrayMultidimensional_Values
    {
        int[,] MyArrayInt { get; set; }
        bool MyArrayInt_Dirty { get; set; }
        int[,,] MyThreeDimArrayInt { get; set; }
        int[][,,][,,,] MyCrazyJaggedArray { get; set; }
    }
}