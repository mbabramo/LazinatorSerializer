using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
	public static class AvlTreeExtensions
	{
		public static string Description<TKey>(this AvlTree<TKey, LazinatorWrapperByte> tree) where TKey : ILazinator, new()
		{
			StringBuilder builder = new StringBuilder();

			Description(builder, tree.Root);

			return builder.ToString();
		}

		public static bool Insert<TKey>(this AvlTree<TKey, LazinatorWrapperByte> source, TKey key) where TKey : ILazinator, new()
        {
			return source.Insert(key, 0);
		}

		public static int Count<TKey>(this AvlTree<TKey, LazinatorWrapperByte> source) where TKey : ILazinator, new()
        {
			AvlNode<TKey, LazinatorWrapperByte> node = source.Root;

			if (node == null)
			{
				return 0;
			}
			else
			{
				return node.Count();
			}
		}

		private static void Description<TKey>(StringBuilder builder, AvlNode<TKey, LazinatorWrapperByte> node) where TKey : ILazinator, new()
        {
			if (node != null)
			{
				builder.Append(node.Key);

				for (int i = 0; i < node.Balance; i++)
				{
					builder.Append("+");
				}

				for (int i = node.Balance; i < 0; i++)
				{
					builder.Append("-");
				}

				if (node.Left != null || node.Right != null)
				{
					builder.Append(":{");

					if (node.Left == null)
					{
						builder.Append("~");
					}
					else
					{
						Description(builder, node.Left);
					}

					builder.Append(",");

					if (node.Right == null)
					{
						builder.Append("~");
					}
					else
					{
						Description(builder, node.Right);
					}

					builder.Append("}");
				}
			}
		}
	}
}
