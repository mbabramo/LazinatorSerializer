using CountedTree.Core;
using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace CountedTree.PendingChanges
{

    public partial class PendingChangeStatus<TKey> : IPendingChangeStatus<TKey> where TKey : struct, ILazinator,
      IComparable,
      IComparable<TKey>,
      IEquatable<TKey>
    {

        public PendingChangeStatus(TKey value, bool delete)
        {
            Process(value, delete);
        }

        public void ProcessDeletion(TKey deletionValue)
        {
            if (deletionValue.Equals(AdditionValue))
            { // deletion value offsetting addition
            }
            else
            {
                AddDeletionValue(deletionValue);
            }
            AdditionValue = null;
        }

        private void AddDeletionValue(TKey deletionValue)
        {
            if (DeletionValues == null)
                DeletionValues = new HashSet<TKey>();
            DeletionValues.Add(deletionValue);
        }

        public void ProcessAddition(TKey additionValue)
        {
            if (DeletionValues != null && DeletionValues.Contains(additionValue) && AdditionValue == null)
            { // this addition is undoing a previous deletion, so clear them both. 
                AdditionValue = null;
                DeletionValues.Remove(additionValue);
            }
            else
                AdditionValue = additionValue; // replaces anything added earlier
        }

        public void Process(TKey value, bool delete)
        {
            if (delete)
                ProcessDeletion(value);
            else
                ProcessAddition(value);
        }

        public IEnumerable<PendingChange<TKey>> GetChanges(uint ID)
        {
            if (DeletionValues != null)
                foreach (var deletionValue in DeletionValues)
                    yield return new PendingChange<TKey>(new KeyAndID<TKey>(deletionValue, ID), true);
            if (AdditionValue != null)
                yield return new PendingChange<TKey>(new KeyAndID<TKey>((TKey)AdditionValue, ID), false);
        }

    }
}
