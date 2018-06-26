using System;
using System.Collections.Generic;
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
        public static ILazinator GetRoot(this ILazinator node)
        {
            while (node.LazinatorParentsReference.LastAdded != null)
                node = node.LazinatorParentsReference.LastAdded;
            return node;
        }
    }
}
