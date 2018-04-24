using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.Array_Values)]
    public interface IArray_Values
    {
        int[] MyArrayInt { get; set; }
        bool MyArrayInt_Dirty { get; set; }
        int[][] MyJaggedArrayInt { get; set; }
    }
}