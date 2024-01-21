using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorTests.Examples;

namespace LazinatorAvlCollectionsTests.LazinatorObjects
{
    [Lazinator((int)CollectionsTestsObjectIDs.IIntInClass)]
    public interface IIntInClass
    {
        int WrappedValue { get; set; }
    }
}