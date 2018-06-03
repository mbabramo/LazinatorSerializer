using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class GenericFromBase<T> : Base, IGenericFromBase<T> where T : ILazinator
    {
    }
}
