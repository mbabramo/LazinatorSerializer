using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Utilities
{
    public class BoolPermutations : IEnumerable<object[]>
    {
        int NumBoolValues = 2;

        public BoolPermutations(int numBoolValues)
        {
            NumBoolValues = numBoolValues;
        }

        bool[][] GeneratePermutations()
        {
            return Enumerable.Range(0, (int)Math.Pow(2, NumBoolValues))
                .Select(i =>
                    Enumerable.Range(0, NumBoolValues)
                        .Select(b => ((i & (1 << b)) > 0))
                        .ToArray()
                ).ToArray();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            var permutations = GeneratePermutations();
            foreach (var permutation in permutations)
            {
                var asObject = permutation.Select(x => (object)x).ToArray();
                yield return asObject;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class BoolPermutations_2 : BoolPermutations
    {
        public BoolPermutations_2() : base(2)
        {

        }
    }

    public class BoolPermutations_3 : BoolPermutations
    {
        public BoolPermutations_3() : base(3)
        {

        }
    }
    public class BoolPermutations_4 : BoolPermutations
    {
        public BoolPermutations_4() : base(4)
        {

        }
    }
    public class BoolPermutations_5 : BoolPermutations
    {
        public BoolPermutations_5() : base(5)
        {

        }
    }
    public class BoolPermutations_6 : BoolPermutations
    {
        public BoolPermutations_6() : base(6)
        {

        }
    }
}
