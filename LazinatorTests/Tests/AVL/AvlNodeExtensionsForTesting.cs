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
	public static class AvlNodeExtensionsForTesting
	{
		public static int CountByEnumerating<TKey>(this AvlNode<TKey, WByte> source) where TKey : ILazinator, new()
		{
			int count = 1;

			AvlNode<TKey, WByte> left = source.Left;
			AvlNode<TKey, WByte> right = source.Right;

			if (right != null)
			{
				count += CountByEnumerating(left);
			}

			if (right != null)
			{
				count += CountByEnumerating(right);
			}

			return count;
		}
	}
}
