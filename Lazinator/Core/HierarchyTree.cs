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
        public List<HierarchyTree> Children;

        public HierarchyTree(ILazinator node)
        {
            Node = node;
        }

        public HierarchyTree(IEnumerable<ILazinator> enumerationOfTree)
        {
            if (!enumerationOfTree.Any())
                return;
            bool first = true;
            foreach (var node in enumerationOfTree)
            {
                if (first)
                {
                    Node = node;
                    first = false;
                }
                else
                    Add(node);
            }
        }

        public void Add(ILazinator node)
        {
            List<ILazinator> hierarchyFromTop = LazinatorUtilities.GetClassAncestorsFromTop(node);
            var enumerator = hierarchyFromTop.GetEnumerator();
            bool more = enumerator.MoveNext();
            if (!more || enumerator.Current != Node)
                throw new Exception("Attempted adding a node that does not belong in this hierarchy.");
            AddToHere(enumerator, node);
        }

        public void AddToHere(IEnumerator<ILazinator> hierarchyFromHere, ILazinator nodeToAdd)
        {
            if (Children == null)
                Children = new List<HierarchyTree>();
            ILazinator childNode;
            if (hierarchyFromHere.MoveNext())
                childNode = hierarchyFromHere.Current;
            else
                childNode = nodeToAdd;
            HierarchyTree childTree = Children.FirstOrDefault(x => x.Node == childNode);
            if (childTree == null)
            {
                childTree = new HierarchyTree(childNode);
                Children.Add(childTree);
            }
            else
                childTree.AddToHere(hierarchyFromHere, nodeToAdd);
        }

        public string ToString(Func<ILazinator, string> stringProducer = null)
        {
            StringBuilder sb = new StringBuilder();
            BuildString(sb, stringProducer, 0);
            return sb.ToString();
        }

        private void BuildString(StringBuilder sb, Func<ILazinator, string> stringProducer = null, int tabs = 0)
        {
            if (stringProducer == null)
                stringProducer = x => x.ToString();
            string textToAppend = stringProducer(Node);
            for (int i = 0; i < tabs * 4; i++)
                sb.Append(" ");
            sb.AppendLine(textToAppend);
            if (Children != null)
            {
                foreach (var child in Children)
                    child.BuildString(sb, stringProducer, tabs + 1);
            }
            
        }
    }
}
