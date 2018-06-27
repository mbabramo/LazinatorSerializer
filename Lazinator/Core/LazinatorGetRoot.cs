using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Core
{
    public static class LazinatorGetRoot
    {
        /// <summary>
        /// Returns the root of a hierarchy if the hierarchy consists of all classes. Otherwise, it returns the highest class containing the node (or the node itself, if it is a struct.)
        /// Where a node has multiple parents, the hierarchy is considered to be the last added parent.
        /// </summary>
        /// <param name="node">A Lazinator object that may be part of a hierarchy</param>
        /// <returns></returns>
        public static ILazinator GetPrincipalRoot(this ILazinator node)
        {
            while (node.LazinatorParents.LastAdded != null)
                node = node.LazinatorParents.LastAdded;
            return node;
        }

        /// <summary>
        /// Returns the hierarchy root if there is a distinct root, or null if there are multiple roots.
        /// </summary>
        /// <param name="node">A Lazinator object that may be part of a hierarchy</param>
        /// <returns></returns>
        public static ILazinator GetSoleRoot(this ILazinator node)
        {
            do
            {
                int count = node.LazinatorParents.Count;
                if (count == 0)
                    return node;
                if (count > 1)
                    return null;
                node = node.LazinatorParents.LastAdded ?? node.LazinatorParents.EnumerateParents().First();
            } while (true);
        }

        /// <summary>
        /// Returns all hierarchy roots for a node. There may be more than one root if node or one or more of its
        /// ancestors has more than one parent. Note that the same root may be returned more than once.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<ILazinator> GetAllRoots(this ILazinator node)
        {
            Queue<ILazinator> q = new Queue<ILazinator>();
            q.Enqueue(node);
            while (q.Any())
            {
                node = q.Dequeue();
                if (node.LazinatorParents.Count == 0)
                    yield return node;
                foreach (var parent in node.LazinatorParents.EnumerateParents())
                    q.Enqueue(parent);
            }
        }

        /// <summary>
        /// Finds each node that is the closest ancestor of the specified type on a path from the node to a hierarchy root.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllClosestAncestorsOfType<T>(this ILazinator node) where T : class, ILazinator
        {
            Queue<ILazinator> q = new Queue<ILazinator>();
            q.Enqueue(node);
            while (q.Any())
            {
                node = q.Dequeue();
                if (node is T correctTypeNode)
                    yield return correctTypeNode;
                else
                {
                    foreach (var parent in node.LazinatorParents.EnumerateParents())
                        q.Enqueue(parent);
                }
            }
        }

        /// <summary>
        /// Returns the sole node that is of the specified type and is the closest on the path from the node to the root.
        /// Throws if there are multiple paths to a hierarchy root with different closest ancestors of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T GetSoleClosestAncestorOfType<T>(this ILazinator node) where T : class, ILazinator
        {
            ILazinator startPosition = node;
            // first, see if we can get from this node to an ancestor of the specified type without hitting a place where there might be two ancestors
            while (node != null && node.LazinatorParents.Count == 1)
            {
                node = node.LazinatorParents.LastAdded;
                if (node is T correctTypeNode)
                    return correctTypeNode;
            }
            // now, we need to use a more cumbersome procedure
            List<T> matches = GetAllClosestAncestorsOfType<T>(startPosition).ToList();
            if (!matches.Any())
                return default(T);
            if (matches.Count() == 1)
                return matches.First();
            throw new Exception($"Expected there to be a sole matching ancestor of type {typeof(T)} but there were {matches.Count()}.");
        }

        /// <summary>
        /// Confirm that two nodes have distinct roots. This is useful as a way to ensure that the same node is not added
        /// in multiple places in the same hierarchy.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        public static void VerifyDistinctRoots(ILazinator node1, ILazinator node2)
        {
            // Try to verify with distinct roots
            ILazinator soleRoot1 = node1.GetSoleRoot();
            if (soleRoot1 != null)
            {
                ILazinator soleRoot2 = node2.GetSoleRoot();
                if (soleRoot2 != null)
                {
                    if (soleRoot1 != soleRoot2)
                        throw new Exception("Nodes were expected to have distinct roots but did not.");
                    return;
                }
            }
            // At least one of these nodes has multiple roots, so we need to use a more expensive mechanism.
            HashSet<ILazinator> h1 = new HashSet<ILazinator>(node1.GetAllRoots());
            HashSet<ILazinator> h2 = new HashSet<ILazinator>(node2.GetAllRoots());
            h1.IntersectWith(h2);
            if (h1.Any())
                throw new Exception("Nodes were expected to have distinct roots but did not.");
        }
    }
}
