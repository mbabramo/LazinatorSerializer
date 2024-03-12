using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Core;

namespace CountedTree.UintSets
{
    public partial class UintSetDeltasLoc : IUintSetDeltasLoc
    {

        public UintSetDeltasLoc()
        {
            ToAdd = new Dictionary<uint, byte>();
            IndicesToRemove = new HashSet<WUInt32>();
        }

        public UintSetDeltasLoc(Dictionary<uint, byte> toAdd, HashSet<WUInt32> indicesToRemove)
        {
            ToAdd = toAdd;
            IndicesToRemove = indicesToRemove;
        }

        public UintSetDeltasLoc(List<WUInt32> indicesToAdd, List<byte> locationsToAdd, ICollection<WUInt32> indicesToRemove)
        {
            var toAddDict = new Dictionary<uint, byte>();
            foreach (var item in indicesToAdd.Zip(locationsToAdd, (i, l) => new KeyValuePair<uint, byte>(i, l)))
                toAddDict[item.Key] = item.Value;
            ToAdd = toAddDict;

            if (indicesToRemove.Any(x => ToAdd.ContainsKey(x)))
                throw new Exception("Internal error. Cannot include an index to both add and remove in the UintSetLocDeltas"); // it would be ambiguous whether this means to remove what is there and then add, or to add and then remove. Note that with regulra UintSetDeltas, this just means to keep the status quo. But because we can have location changes, this doesn't make sense anymore.
            IndicesToRemove = new HashSet<WUInt32>(indicesToRemove.Select(x => x.CloneLazinatorTyped()));
        }

        internal UintSetDeltasLoc Changing_ToAdd(Dictionary<uint, byte> toAdd)
        {
            return new UintSetDeltasLoc(toAdd, IndicesToRemove);
        }

        internal UintSetDeltasLoc Changing_IndicesToRemove(HashSet<WUInt32> indicesToRemove)
        {
            return new UintSetDeltasLoc(ToAdd, indicesToRemove);
        }

        public UintSetWithLoc IntegrateIntoUintSetLoc(UintSet previousUintSet, UintSetLoc uintSetLocToIntegrateInto)
        {
            UintSet replacementUintSet = previousUintSet.Clone();
            var indicesToAdd = ToAdd.Select(x => (WUInt32)x.Key).ToList();
            var locationsToAdd = ToAdd.Select(x => x.Value).ToList();
            var replacementUintSetLoc = uintSetLocToIntegrateInto.AddItemsAtUintIndices(previousUintSet, indicesToAdd, locationsToAdd);
            replacementUintSet.AddUints(indicesToAdd);
            replacementUintSetLoc = replacementUintSetLoc.RemoveItemsAtUintIndices(replacementUintSet, IndicesToRemove);
            replacementUintSet.RemoveUints(IndicesToRemove);
            return new UintSetWithLoc(replacementUintSet, replacementUintSetLoc);
        }

        public UintSetDeltasLoc Update(List<WUInt32> indicesToAdd, List<byte> locationsToAdd, List<WUInt32> indicesToRemove)
        {
            var itemsToAddDictionary = indicesToAdd.Zip(locationsToAdd, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
            HashSet<WUInt32> itemsToRemoveHashSet = new HashSet<WUInt32>(indicesToRemove);
            return Update(itemsToRemoveHashSet, itemsToAddDictionary);
        }

        public UintSetDeltasLoc Update(HashSet<WUInt32> itemsToRemoveHashSet, Dictionary<WUInt32, byte> itemsToAddDictionary)
        {
            UintSetDeltasLoc updated = this.CloneLazinatorTyped();
            foreach (var item in itemsToAddDictionary)
            {
                // If we had previously planned to remove an item, but now we are adding it, we remove it from our list of IndicesToRemove.
                //
                updated.IndicesToRemove.Remove(item.Key); // will ignore if not present
                // Either change or add this item to our ToAdd list. 
                updated.ToAdd[item.Key] = item.Value;
            }
            foreach (var item in itemsToRemoveHashSet)
            {
                // if we have an index being removed and then added with another value, we simply ignore the removal (because we don't want to remove what we are adding). This should still succeed in setting the delta loc.
                if (!itemsToAddDictionary.ContainsKey(item))
                {
                    // We're deleting an index that we're not also adding. We start by removing it from the delta set's list of things to add, since we know that we're really deleting it.
                    // Further explanation: Suppose we have index 7, child 20 in main set. Then, we change it, so we specify index 7, child 4 in delta set. Now, it's in ToAdd. If index 7 is now to be removed altogether, then we can't simply take index 7, child 4 out of the delta set, because that would leave index 7 at child 20. Instead, we must specify that this is an item to be removed. Note that an implication of this is that some items in IndicesToRemove may not actually be in the main set, but this shouldn't create any problems for us.
                    updated.ToAdd.Remove(item);
                    updated.IndicesToRemove.Add(item);
                }
            }
            return updated;
        }
    }
}
