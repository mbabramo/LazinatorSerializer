using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    public static class LazinatorGetRoot
    {
        /// <summary>
        /// Returns the root of a hierarchy if the hierarchy consists of all classes. Otherwise, it returns the highest class containing the node (or the node itself, if it is a struct.)
        /// </summary>
        /// <param name="node">A Lazinator object that may be part of a hierarchy</param>
        /// <returns></returns>
        public static ILazinator GetRoot(this ILazinator node)
        {
            while (node.LazinatorParentClass.FirstOrDefault() != null)
                node = node.LazinatorParentClass.FirstOrDefault();
            return node;
        }
    }
}
