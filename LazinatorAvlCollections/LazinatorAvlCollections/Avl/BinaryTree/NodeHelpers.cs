using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    public static class NodeHelpers
    {
        public static N GetNextNode<T, N>(this N node) where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
        {
            // All the nodes to the left are complete. Therefore, if there is a node to the right, we move to the right and then as far to the left as possible. Otherwise, we move to the first parent where this is on the left; if there is no such parent, we return null, because there is no last node.
            N current = node;
            if (current.Right != null)
            {
                current = current.Right;
                while (current.Left != null)
                {
                    current = current.Left;
                }
                return current;
            }
            while (true)
            {
                var p = current.Parent;
                if (p == null)
                {
                    return null;
                }
                if (p.Left == current)
                    return p;
                current = p;
            }
        }

        public static N GetPreviousNode<T, N>(this N node) where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
        {
            // If there is a left node, then we just came from its rightmost descendant. 
            N current = node;
            if (current.Left != null)
            {
                current = current.Left;
                while (current.Right != null)
                    current = current.Right;
                return current;
            }
            var p = current.Parent;
            // If there is no parent, then this is a root node with no children, and thus the first node.
            if (p == null)
                return null;
            // If the parent is to the left (i.e., this is the right child), then that was previous.
            if (p.Right == current)
            {
                current = p;
                return current;
            }
            // Otherwise, go up to the right as far as possible and then one up to the left.
            while (p != null && p.Left == current)
            {
                current = p;
                p = current.Parent;
            }
            return p;
        }
        
        public static string ToTreeString<T, N>(this N node) where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
        {
            StringBuilder sb = new StringBuilder();
            node.ToTreeStringHelper<T, N>(sb, "", false);
            return sb.ToString();
        }

        private static void ToTreeStringHelper<T, N>(this N node, StringBuilder sb, string indent, bool right) where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
        {
            sb.Append(indent);
            if (right)
            {
                sb.Append("\\-");
                indent += "  ";
            }
            else
            {
                sb.Append("|-");
                indent += "| ";
            }

            string thisNode = node.Value is ITreeString treeString ? treeString.ToTreeString() : node.ToString();
            if (thisNode.Contains("\n"))
            {
                thisNode = thisNode.Replace("\r\n", "\n");
                thisNode = BoxAroundString(thisNode);
                thisNode = thisNode.Replace("\n", "\n" + indent);
            }

            sb.AppendLine(thisNode);

            node.Left?.ToTreeStringHelper<T, N>(sb, indent, false);
            node.Right?.ToTreeStringHelper<T, N>(sb, indent, true);
        }

        private static string BoxAroundString(string s)
        {
            List<string> lines = s.Split('\n').ToList();
            int length = lines.Max(x => x.Length);
            var lines2 = lines.Select(x => "║ " + x + new string(' ', length - x.Length) + " ║").ToList();
            lines2.Insert(0, "╔═" + new string('═', length) + "═╗");
            lines2.Add("╚═" + new string('═', length) + "═╝");
            string box = String.Join("\n", lines2);
            return box;
        }
    }
}
