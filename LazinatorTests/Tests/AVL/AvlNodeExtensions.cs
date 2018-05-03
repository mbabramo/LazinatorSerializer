﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Collections.Avl;
using Lazinator.Core;

namespace LazinatorTests.AVL
{
	public static class AvlNodeExtensions
	{
		public static int Count<TKey>(this AvlNode<TKey, TKey> source) where TKey : ILazinator
		{
			int count = 1;

			AvlNode<TKey, TKey> left = source.Left;
			AvlNode<TKey, TKey> right = source.Right;

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
