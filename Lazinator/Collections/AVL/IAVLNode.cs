using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Collections.AVL
{
    interface IAvlNode<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        AvlNode<TKey, TValue> Left { get; set; }
        AvlNode<TKey, TValue> Right { get; set; }
        TKey Key { get; set; }
        TValue Value { get; set; }
        int Balance { get; set; }
    }
}
