using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Core
{
    /// <summary>
    /// A representation of a hierarchy of nodes, useful for creating a string representing the structure of a Lazinator hierarchy.
    /// </summary>
    public class HierarchyTree
    {
        public ILazinator Node;
        public List<(string propertyName, object propertyValue)> NonLazinatorProperties;
        public List<(string childName, HierarchyTree subtree)> Children;

        public HierarchyTree(ILazinator node)
        {
            Node = node;
            if (node == null)
                return;
            NonLazinatorProperties = node.EnumerateNonLazinatorProperties().ToList();
            Children = node.EnumerateLazinatorChildren().Select(x => (x.propertyName, new HierarchyTree(x.descendant))).ToList();
        }

        public string ToString(Func<ILazinator, string> stringProducer = null)
        {
            StringBuilder sb = new StringBuilder();
            BuildString(sb, "", stringProducer, 0);
            return sb.ToString();
        }

        private void BuildString(StringBuilder sb, string prefix, Func<ILazinator, string> stringProducer = null, int tabs = 0)
        {
            if (stringProducer == null)
                stringProducer = x => x == null ? "NULL" : x.ToString();
            string textToAppend = stringProducer(Node);
            for (int i = 0; i < tabs * 4; i++)
                sb.Append(" ");
            if (prefix != null && prefix != "")
            {
                sb.Append(prefix);
                sb.Append(": ");
            }
            sb.AppendLine(textToAppend);
            if (Node == null)
                return;
            foreach (var property in NonLazinatorProperties)
            {
                for (int i = 0; i < (tabs + 1) * 4; i++)
                    sb.Append(" ");
                sb.Append(property.propertyName);
                sb.Append(": ");
                if (property.propertyValue == null)
                    sb.AppendLine("NULL");
                else
                {
                    if (property.propertyValue is IEnumerable enumerable && !(property.propertyValue is string))
                    {
                        var enumerated = enumerable.OfType<object>().Select(x => x is ILazinator l ? new HierarchyTree(l).ToString() : x);
                        sb.AppendLine(String.Join(", ", enumerated));
                    }
                    else
                        sb.AppendLine(property.propertyValue.ToString());
                }
            }
            foreach (var child in Children)
                child.subtree.BuildString(sb, child.childName, stringProducer, tabs + 1);
            
        }
    }
}
