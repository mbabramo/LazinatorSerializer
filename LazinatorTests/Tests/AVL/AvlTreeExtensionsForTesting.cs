using System.Text;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
    public static class AvlTreeExtensionsForTesting
    {
        public static string Description<TKey>(this AvlTree<TKey, WByte> tree) where TKey : ILazinator
        {
            StringBuilder builder = new StringBuilder();

            Description(builder, tree.Root);

            return builder.ToString();
        }

        public static bool Insert<TKey>(this AvlTree<TKey, WByte> source, TKey key) where TKey : ILazinator
        {
            return source.Insert(key, 0);
        }

        public static int CountByEnumerating<TKey>(this AvlTree<TKey, WByte> source) where TKey : ILazinator
        {
            AvlNode<TKey, WByte> node = source.Root;

            if (node == null)
            {
                return 0;
            }
            else
            {
                return node.CountByEnumerating();
            }
        }

        private static void Description<TKey>(StringBuilder builder, AvlNode<TKey, WByte> node) where TKey : ILazinator
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
