using LazinatorTests.Examples.Tuples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Examples.Subclasses
{
    public partial record LazinatorRecordSubclass : LazinatorRecord, ILazinatorRecordSubclass
    {
    }
}
