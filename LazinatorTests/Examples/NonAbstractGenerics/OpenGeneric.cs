using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class OpenGeneric<T> : IOpenGeneric<T> where T : ILazinator, new()
    {
    }
}
