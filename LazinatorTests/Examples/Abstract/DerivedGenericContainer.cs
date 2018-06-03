using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class DerivedGenericContainer<T> : IDerivedGenericContainer<T> where T : ILazinator, new()
    {
    }
}
