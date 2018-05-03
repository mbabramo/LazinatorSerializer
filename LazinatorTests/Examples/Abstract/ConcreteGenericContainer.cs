using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Buffers;
using Lazinator.Core;

namespace LazinatorTests.Examples.Abstract
{
    public partial class ConcreteGenericContainer : AbstractGenericContainer<int>, IConcreteGenericContainer
    {
    }
}
