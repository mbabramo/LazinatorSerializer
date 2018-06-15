using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Abstract
{
    public partial class ConcreteFromGenericFromBase : GenericFromBase<WNullableDecimal>, IConcreteFromGenericFromBase
    {
    }
}
