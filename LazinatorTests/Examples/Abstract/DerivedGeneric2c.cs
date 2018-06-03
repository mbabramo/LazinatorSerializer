using Lazinator.Buffers;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class DerivedGeneric2c<T> : AbstractGeneric1<T>, IDerivedGeneric2c<T> where T : ILazinator, new()
    {
    }
}
