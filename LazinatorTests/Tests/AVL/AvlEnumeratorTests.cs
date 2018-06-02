using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lazinator.Collections.Avl;
using Lazinator.Wrappers;
using Xunit;

namespace LazinatorTests.AVL
{
	public class AvlEnumeratorTests
	{
		[Fact]
		public void Sorting()
		{
			var tree = SetupTree(5, 4, 3, 2, 1);

			var enumerator = new AvlNodeEnumerator<WInt, WByte>(tree.Root);

			for (int i = 1; i <= 5; i++)
			{
				Assert.True(enumerator.MoveNext());

				Assert.Equal(i, enumerator.Current.Key.WrappedValue);
			}

			Assert.False(enumerator.MoveNext());
			Assert.False(enumerator.MoveNext());
		}

		[Fact]
		public void Reset()
		{
			var tree = SetupTree(5, 4, 3, 2, 1);

			var enumerator = tree.GetEnumerator();

			int count1 = 0;

			while (enumerator.MoveNext())
			{
				count1++;
			}

			enumerator.Reset();

			int count2 = 0;

			while (enumerator.MoveNext())
			{
				count2++;
			}

			Assert.Equal(5, count1);
			Assert.Equal(5, count2);
		}

		[Fact]
		public void TreeForEach()
		{
			var tree = SetupTree(5, 4, 3, 2, 1);

			int count = 0;

			foreach (var node in tree)
			{
				count++;

				Assert.Equal(count, node.Key.WrappedValue);
			}

			Assert.Equal(5, count);
		}

		[Fact]
		public void LegacyEnumerator()
		{
			var tree = SetupTree(5, 4, 3, 2, 1);

			int count = 0;

			IEnumerable enumerable = tree;

			IEnumerator enumerator = enumerable.GetEnumerator();

			while (enumerator.MoveNext())
			{
				count++;

				Assert.Equal(count, ((AvlNode<WInt, WByte>)enumerator.Current).Key.WrappedValue);
			}

			Assert.Equal(5, count);
		}

		private AvlTree<WInt, WByte> SetupTree(params int[] values)
		{
			var tree = new AvlTree<WInt, WByte>();

			foreach (int value in values)
			{
				tree.Insert(value, 0);
			}

			return tree;
		}
	}
}
