using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Examples.Tuples
{
    public record LazinatorRecord : ILazinatorRecord
    {
        public int MyInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MyString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
