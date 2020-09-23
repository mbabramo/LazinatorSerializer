using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Examples.Tuples
{
    public record NonLazinatorRecordWithoutConstructor
    {
        public int Age
        {
            get;
            set;
        }
        public Example Example
        {
            get;
            init;
        }
        public double DoubleValue
        {
            get;
            set;
        }
        public int? NullableInt
        {
            get;
            set;
        }
        public int GetOnly => NullableInt ?? 0; // must make sure that code doesn't try to initialize this
    }
}
