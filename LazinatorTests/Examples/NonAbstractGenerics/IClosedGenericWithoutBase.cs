using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.ClosedGeneric)]
    public interface IClosedGenericWithoutBase : IOpenGenerics<int, ExampleChild>
    {
    }
}
