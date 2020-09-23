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
        public double GetOnly => DoubleValue;
    }
}
