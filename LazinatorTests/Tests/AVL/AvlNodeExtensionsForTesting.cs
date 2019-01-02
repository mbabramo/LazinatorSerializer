using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace LazinatorTests.AVL
{
    public static class AvlNodeExtensionsForTesting
	{
		public static int CountByEnumerating<TKey>(this AvlNode<TKey, Placeholder> source) where TKey : ILazinator, IComparable<TKey>
        {
			int count = 1;

			AvlNode<TKey, Placeholder> left = source.Left;
			AvlNode<TKey, Placeholder> right = source.Right;

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
