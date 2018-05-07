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
		public static int CountByEnumerating<TKey>(this AvlNode<TKey, LazinatorWrapperByte> source) where TKey : ILazinator, new()
		{
			int count = 1;

			AvlNode<TKey, LazinatorWrapperByte> left = source.Left;
			AvlNode<TKey, LazinatorWrapperByte> right = source.Right;

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
