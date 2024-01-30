using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int)ExampleUniqueIDs.SealedClass)]
    public interface ISealedClass
    {
        int MyInt { get; set; }
    }
}
