using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Collections
{
    public partial class LazinatorListContainerGeneric<T> : ILazinatorListContainerGeneric<T> where T : ILazinator, new()
    {
        public LazinatorListContainerGeneric()
        {
        }
    }
}
