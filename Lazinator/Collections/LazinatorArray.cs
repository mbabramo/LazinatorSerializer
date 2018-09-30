using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class LazinatorArray<T> : LazinatorList<T>, ILazinatorArray<T> where T : ILazinator
    {
        // TODO: Consider an implementation in which LazinatorList is an underlying type, instead of throwing exceptions.
        // Or implement LazinatorArray independently, using some LazinatorList functionality in a base class.

        public LazinatorArray()
        {

        }

        public LazinatorArray(int numItems)
        {
            for (int i = 0; i < numItems; i++)
                base.Add(default);
        }

        public LazinatorArray(IEnumerable<T> items)
        {
            foreach (T item in items)
                base.Add(item);
        }

        public class NotSupportedInLazinatorArrayException : Exception
        {
            public NotSupportedInLazinatorArrayException() : base("Operation is not supported on LazinatorArray. Considering using LazinatorList instead")
            {

            }
        }


        public int Length => Count;


        protected override void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren || includeChildrenMode == IncludeChildrenMode.ExcludeOnlyExcludableChildren)
            {
                LazinatorArray<T> typedClone = (LazinatorArray<T>)clone;
                int i = 0;
                foreach (T member in this)
                {
                    if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(member, default(T)))
                        typedClone[i] = default(T);
                    else
                        typedClone[i] = member.CloneLazinatorTyped(includeChildrenMode, CloneBufferOptions.NoBuffer);
                    i++;
                }
            }
        }

        public override void Add(T item) => throw new NotSupportedInLazinatorArrayException();

        public override void Clear() => throw new NotSupportedInLazinatorArrayException();

        public override void Insert(int index, T item) => throw new NotSupportedInLazinatorArrayException();
        public override bool Remove(T item) => throw new NotSupportedInLazinatorArrayException();

        public override int RemoveAll(Predicate<T> match) => throw new NotSupportedInLazinatorArrayException();

        public override void RemoveAt(int index) => throw new NotSupportedInLazinatorArrayException();
    }
}
