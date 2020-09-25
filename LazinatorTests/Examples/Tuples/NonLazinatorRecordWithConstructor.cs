using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Examples.Tuples
{
    public record NonLazinatorRecordWithConstructor(int Age, Example Example, double DoubleValue, int? NullableInt);
}
