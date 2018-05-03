using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
	public sealed partial class AvlNode<TKey, TValue> : IAvlNode<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
		public AvlNode<TKey, TValue> Parent { get; set; }
    }
}
