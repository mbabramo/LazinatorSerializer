using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Core
{
    /// <summary>
    /// A representation of a hierarchy of nodes, useful for modeling the structure of a Lazinator hierarchy.
    /// </summary>
    public class HierarchyTree
    {
        public ILazinator Node;
        public List<(string childName, HierarchyTree subtree)> Children;

        public HierarchyTree(ILazinator node)
        {
            Node = node;
            Children = node.GetLazinatorChildren().Select(x => (x.propertyName, new HierarchyTree(x.descendant))).ToList();
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
                stringProducer = x => x.ToString();
            string textToAppend = stringProducer(Node);
            for (int i = 0; i < tabs * 4; i++)
                sb.Append(" ");
            if (prefix != null && prefix != "")
            {
                sb.Append(prefix);
                sb.Append(": ");
            }
            sb.AppendLine(textToAppend);
            if (Children != null)
            {
                foreach (var child in Children)
                    child.subtree.BuildString(sb, child.childName, stringProducer, tabs + 1);
            }
            
        }
    }
}
