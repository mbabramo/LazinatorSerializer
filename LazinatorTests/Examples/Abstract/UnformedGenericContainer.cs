using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class UnformedGenericContainer<T> : IUnformedGenericContainer<T> where T : ILazinator, new()
    {
    }
}
