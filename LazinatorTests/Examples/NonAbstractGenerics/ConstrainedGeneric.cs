using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ConstrainedGeneric<T, U> : IConstrainedGeneric<T, U> where T : struct, ILazinator where U : ILazinator, new()
    {
    }
}
