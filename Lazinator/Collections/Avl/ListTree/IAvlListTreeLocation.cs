using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Avl.ValueTree;

namespace Lazinator.Collections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListTreeLocation)]
    public interface IAvlListTreeLocation<T> where T : ILazinator
    {
        AvlCountedNode<IMultivalueContainer<T>> OuterNode { get; set; }
        IContainerLocation InnerLocation { get; set; }
    }
}