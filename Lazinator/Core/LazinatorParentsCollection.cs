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
        // A weak reference to the last parent added. Weak references are used so that when objects are removed from a 
        // hierarchy, we do not have a memory leak, with the previous parent's entire hierarchy unable to be garbage
        // collected. 
        private readonly WeakReference<ILazinator> LastAddedReference;

        // A complication is that an object can have the same parent more than once (that is, two childs of an
        // object can be the same), so we have to keep track of the number of times a parent is a parent.
        // Since usually this will only have one or two items at most, we use a linked list rather than a dictionary.
        private readonly LinkedList<(WeakReference<ILazinator> parent, int count)> OtherParents;


        /// <summary>
        /// The last Lazinator parent added. If the last added is subsequently removed, this will 
        /// be null, even if some other parents were added earlier.
        /// </summary>
        public ILazinator LastAdded => GetWeakReferenceTargetOrNull(LastAddedReference);

        private ILazinator GetWeakReferenceTargetOrNull(WeakReference<ILazinator> weakLazinator)
        {
            if (weakLazinator != null && weakLazinator.TryGetTarget(out ILazinator reference))
            {
                return reference;
            }

            return null;
        }

        private readonly int LastAddedCount;

        /// <summary>
        /// Returns true if any parents exist in the collection and have not been garbage collected. Note that they might be garbage collected immediately after a call to this function.
        /// </summary>
        public bool Any() => LastAdded != null || (OtherParents != null && OtherParents.Any());

        /// <summary>
        /// Returns true if any parents exist in the collection, have not been garbage collected, and meet the predicate. Note that they might be garbage collected immediately after a call to this function.
        /// </summary>
        public bool Any(Func<ILazinator, bool> predicate)
        {
            if (LastAdded is ILazinator nonNull && predicate(nonNull))
                return true;
            if (OtherParents != null && OtherParents.Any(x => x.parent != null && x.parent.TryGetTarget(out ILazinator stillExisting) && predicate(stillExisting)))
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

        public LazinatorParentsCollection(ILazinator lastAdded, LinkedList<(WeakReference<ILazinator> parent, int count)> otherParents = null)
        {
            LastAddedReference = new WeakReference<ILazinator>(lastAdded);
            LastAddedCount = (lastAdded == null) ? 0 : 1;
            var filtered = otherParents == null ? null : new LinkedList<(WeakReference<ILazinator> parent, int count)>(otherParents.Where(x => x.parent is WeakReference<ILazinator> w && w.TryGetTarget(out ILazinator resolvedTarget)));
            OtherParents = filtered;
        }

        public LazinatorParentsCollection(ILazinator lastAdded, int lastAddedCount, LinkedList<(WeakReference<ILazinator> parent, int count)> otherParents = null)
        {
            LastAddedReference = new WeakReference<ILazinator>(lastAdded);
            LastAddedCount = lastAddedCount;
            var filtered = otherParents == null ? null : new LinkedList<(WeakReference<ILazinator> parent, int count)>(otherParents.Where(x => x.parent is WeakReference<ILazinator> w && w.TryGetTarget(out ILazinator resolvedTarget)));
            OtherParents = filtered;
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

        private LinkedListNode<(WeakReference<ILazinator> parent, int count)> GetNodeWithParent(ILazinator parent)
        {
            if (OtherParents == null || !OtherParents.Any())
                return null;
            var node = OtherParents.First;
            while (node != null)
            {
                ILazinator nodeParentTarget = GetWeakReferenceTargetOrNull(node.Value.parent);
                if (nodeParentTarget == parent)
                    return node;
                node = node.Next;
            }
            return null;
        }

        public LazinatorParentsCollection WithAdded(ILazinator parent) =>
            WithAdded(new WeakReference<ILazinator>(parent));

        public LazinatorParentsCollection WithAdded(WeakReference<ILazinator> parent)
        {
            if (parent == null)
                return this;

            ILazinator parentTarget = GetWeakReferenceTargetOrNull(parent);
            if (parentTarget == null)
                return this;

            ILazinator lastAdded = LastAdded;
            if (lastAdded == null)
            {
                if (OtherParents == null)
                    return new LazinatorParentsCollection(parentTarget);
                var node = GetNodeWithParent(parentTarget);
                if (node != null)
                {   
                    var otherParentsWithRemoval = OtherParents;
                    int revisedCount = node.Value.count + 1;
                    otherParentsWithRemoval.Remove(node);
                    return new LazinatorParentsCollection(parentTarget, revisedCount, otherParentsWithRemoval);
                }
                return new LazinatorParentsCollection(parentTarget, OtherParents);
            }

            if (parentTarget == lastAdded)
                return new LazinatorParentsCollection(parentTarget, LastAddedCount + 1, OtherParents);

            // move LastAdded to OtherParents
            var otherParentsWithAddition = OtherParents;
            if (otherParentsWithAddition == null)
                otherParentsWithAddition = new LinkedList<(WeakReference<ILazinator> parent, int count)>();
            otherParentsWithAddition.AddFirst((LastAddedReference, LastAddedCount)); // LastAddedReference is guaranteed to be good because we have a copy of its target, lastAdded.
            return new LazinatorParentsCollection(parentTarget, otherParentsWithAddition);
        }

        public LazinatorParentsCollection WithRemoved(ILazinator parent)
        {
            var lastAdded = LastAdded;
            if (LastAddedReference != null && lastAdded == null) // no longer valid reference
            {
                var filtered = OtherParents == null ? null : new LinkedList<(WeakReference<ILazinator> parent, int count)>(OtherParents.Where(x => x.parent is WeakReference<ILazinator> w && w.TryGetTarget(out ILazinator resolvedTarget) && resolvedTarget != parent));
                return new LazinatorParentsCollection(null, filtered);
            }

            if (lastAdded == parent)
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
            return new LazinatorParentsCollection(lastAdded, otherParents);
        }

        public void InformParentsOfDirtiness()
        {
            var lastAdded = LastAdded;
            if (lastAdded != null)
                lastAdded.DescendantIsDirty = true;
            if (OtherParents != null)
                foreach (var parentWithCount in OtherParents)
                {
                    var parentTarget = GetWeakReferenceTargetOrNull(parentWithCount.parent); 
                    if (parentTarget != null && parentTarget != lastAdded)
                        parentTarget.DescendantIsDirty = true;
                }
        }

        public IEnumerable<ILazinator> EnumerateParents()
        {
            var lastAdded = LastAdded;
            if (lastAdded != null)
                yield return lastAdded;
            if (OtherParents != null)
                foreach (var parentWithCount in OtherParents)
                {
                    var parentTarget = GetWeakReferenceTargetOrNull(parentWithCount.parent);
                    if (parentTarget != null && parentTarget != lastAdded)
                        yield return parentTarget;
                }
        }
    }
}

