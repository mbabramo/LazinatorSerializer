using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.DerivedLazinatorList)]
    public interface IDerivedLazinatorList<T> : ILazinatorList<T> where T : ILazinator
    {
        string MyListName { get; set; }
    }
}
