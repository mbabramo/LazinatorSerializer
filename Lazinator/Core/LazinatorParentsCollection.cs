using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Core
{
    /// <summary>
    /// Tracks the classes that are parents of a Lazinator class or struct.
    /// It can be used to add or remove a parent, or to notify all parents when the child becomes dirty.
    /// When the parent is a struct, the parent is not tracked. In that case, the parent struct's value can be changed only 
    /// when setting the value of the struct, so that will generate the notification of dirtiness.
    /// </summary>
    public readonly struct LazinatorParentsCollection
    {
        // A complication is that an object can have the same parent more than once (that is, two childs of an
        // object can be the same), so we have to keep track of the number of times a parent is a parent.
        // Since usually this will only have one or two items at most, we use a linked list rather than a dictionary.
        private readonly LinkedList<(ILazinator parent, int count)> OtherParents;

        /// <summary>
        /// The last Lazinator parent added. If the last added is subsequently removed, this will 
        /// be null, even if some other parents were added earlier.
        /// </summary>
        public readonly ILazinator LastAdded;

        private readonly int LastAddedCount;

        public bool Any() => LastAdded != null || (OtherParents != null && OtherParents.Any());

        public bool Any(Func<ILazinator, bool> predicate)
        {
            if (LastAdded != null && predicate(LastAdded))
                return true;
            if (OtherParents != null && OtherParents.Any(x => predicate(x.parent)))
                return true;
            return false;
        }

        /// <summary>
        /// Indicates whether the Lazinator object's buffer is shared by its parent.
        /// </summary>
        /// <param name="ownedMemory">The memory buffer of the Lazinator object to which this parents collection belongs.</param>
        /// <returns></returns>
        public bool ParentSharesBuffer(IMemoryOwner<byte> ownedMemory)
        {
            return Any(x => x.LazinatorMemoryStorage.OwnedMemory == ownedMemory);
        }

        /// <summary>
        /// The number of parents stored. A parent is counted only once even if stored more often.
        /// </summary>
        public int Count => (LastAdded == null ? 0 : 1) + (OtherParents?.Count() ?? 0);

        public LazinatorParentsCollection(ILazinator lastAdded, LinkedList<(ILazinator parent, int count)> otherParents = null)
        {
            LastAdded = lastAdded;
            LastAddedCount = (lastAdded == null) ? 0 : 1;
            OtherParents = otherParents;
        }

        public LazinatorParentsCollection(ILazinator lastAdded, int lastAddedCount, LinkedList<(ILazinator parent, int count)> otherParents = null)
        {
            LastAdded = lastAdded;
            LastAddedCount = lastAddedCount;
            OtherParents = otherParents;
        }

        public T GetSoleParentOfType<T>() where T : class, ILazinator
        {
            T found = null;
            if (LastAdded != null)
                if (LastAdded is T ofCorrectType)
                    found = ofCorrectType;
            if (OtherParents != null)
                foreach (var node in OtherParents)
                    if (node is T ofCorrectType)
                    {
                        if (found == null)
                            found = ofCorrectType;
                        else
                            throw new Exception($"Only one parent of type {typeof(T)} was expected, but more than one was found.");
                    }
            return found;
        }

        private LinkedListNode<(ILazinator parent, int count)> GetNodeWithParent(ILazinator parent)
        {
            if (OtherParents == null || !OtherParents.Any())
                return null;
            var node = OtherParents.First;
            while (node != null)
            {
                if (node.Value.parent == parent)
                    return node;
                node = node.Next;
            }
            return null;
        }

        public LazinatorParentsCollection WithAdded(ILazinator parent)
        {
            if (parent == null)
                return this;

            if (LastAdded == null)
            {
                if (OtherParents == null)
                    return new LazinatorParentsCollection(parent);
                var node = GetNodeWithParent(parent);
                if (node != null)
                {   
                    var otherParentsWithRemoval = OtherParents;
                    int revisedCount = node.Value.count + 1;
                    otherParentsWithRemoval.Remove(node);
                    return new LazinatorParentsCollection(parent, revisedCount, otherParentsWithRemoval);
                }
                return new LazinatorParentsCollection(parent, OtherParents);
            }

            if (parent == LastAdded)
                return new LazinatorParentsCollection(parent, LastAddedCount + 1, OtherParents);

            // move LastAdded to OtherParents
            var otherParentsWithAddition = OtherParents;
            if (otherParentsWithAddition == null)
                otherParentsWithAddition = new LinkedList<(ILazinator parent, int count)>();
            otherParentsWithAddition.AddFirst((LastAdded, LastAddedCount));
            return new LazinatorParentsCollection(parent, otherParentsWithAddition);
        }

        public LazinatorParentsCollection WithRemoved(ILazinator parent)
        {
            if (LastAdded == parent)
            { 
                if (LastAddedCount > 1)
                {
                    return new LazinatorParentsCollection(parent, LastAddedCount - 1, OtherParents);
                }
                else
                    return new LazinatorParentsCollection(null, OtherParents);
            }
            var node = GetNodeWithParent(parent);
            if (node == null)
                return this; // nothing to remove
            var otherParents = OtherParents;
            if (node.Value.count == 1)
                otherParents.Remove(node);
            else
                node.Value = (node.Value.parent, node.Value.count - 1);
            return new LazinatorParentsCollection(LastAdded, otherParents);
        }

        public void InformParentsOfDirtiness()
        {
            if (LastAdded != null)
                LastAdded.DescendantIsDirty = true;
            if (OtherParents != null)
                foreach (var parent in OtherParents)
                {
                    if (parent.parent != LastAdded)
                        parent.parent.DescendantIsDirty = true;
                }
        }

        public IEnumerable<ILazinator> EnumerateParents()
        {
            if (LastAdded != null)
                yield return LastAdded;
            if (OtherParents != null)
                foreach (var parent in OtherParents)
                {
                    if (parent.parent != LastAdded)
                        yield return parent.parent;
                }
        }
    }
}
