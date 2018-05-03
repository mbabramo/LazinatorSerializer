using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
	public sealed class AvlNode<TKey, TValue> : IAvlNode<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
	{
		public AvlNode<TKey, TValue> Parent { get; set; }

        public AvlNode<TKey, TValue> Left { get; set; }
        public AvlNode<TKey, TValue> Right { get; set; }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public int Balance { get; set; }
    }
}
