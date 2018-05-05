using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
	public static class AvlNodeExtensions
	{
		public static int Count<TKey>(this AvlNode<TKey, LazinatorWrapperByte> source) where TKey : ILazinator, new()
		{
			int count = 1;

			AvlNode<TKey, LazinatorWrapperByte> left = source.Left;
			AvlNode<TKey, LazinatorWrapperByte> right = source.Right;

			if (right != null)
			{
				count += Count(left);
			}

			if (right != null)
			{
				count += Count(right);
			}

			return count;
		}
	}
}
