using Lazinator.Core;
using Lazinator.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.TypeAhead
{
    /// <summary>
    /// This maintains information associated with a specific set of characters that the user may type in. Caching the top results here allows us to quickly return suggestions.
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public partial class TypeAheadNode<R> : ITypeAheadNode<R> where R : ILazinator,
          IComparable,
          IComparable<R>,
          IEquatable<R>
    {
        public TypeAheadItem<R> LowestItem => TypeAheadItems.Last();

        public TypeAheadNode(string stringToHere, int minThreshold, int maxThreshold)
        {
            StringToHere = stringToHere;
            TypeAheadItems = new List<TypeAheadItem<R>>();
            NextChars = new char[0];
            MinThreshold = minThreshold;
            MaxThreshold = maxThreshold;
            ExactMatches = new LazinatorSortedList<TypeAheadItem<R>>();
        }

        public List<R> GetResults(int numToTake)
        {
            return TypeAheadItems.Take(numToTake).Select(x => x.SearchResult).ToList();
        }

        public List<TypeAheadItem<R>> GetTypeAheadItems(int numToTake, bool prioritizeExactMatches)
        {
            if (prioritizeExactMatches)
                return ExactMatches.Concat(TypeAheadItems.Where(x => !ExactMatches.Contains(x))).Take(numToTake).ToList();
            else
                return TypeAheadItems.Take(numToTake).ToList();
        }

        public TypeAheadItemsForParent<R> GetItemsForParent(TypeAheadItem<R> lowestItemOnParent, string searchString, int maxNumResults)
        {
            return new TypeAheadItemsForParent<R>(this, searchString, lowestItemOnParent, maxNumResults);
        }

        private int? GetResultIndex(R searchResult)
        {
            int i = 0;
            foreach (TypeAheadItem<R> existing in TypeAheadItems)
            {
                if (existing.SearchResult.Equals(searchResult))
                    return i;
                i++;
            }
            return null;
        }

        public override string ToString()
        {
            return String.Join(", ", TypeAheadItems) + " REJECTED: " + $"{MostPopularRejected}";
        }

        public void AddResultsFromChildren(List<TypeAheadItem<R>> typeAheadItem)
        {
            MostPopularRejected = null;
            foreach (var result in typeAheadItem)
                AddResult(null, result, true);
        }

        public bool AddResult(char? nextChar, TypeAheadItem<R> typeAheadItem, bool addedFromChildren = false)
        {
            bool result = AddResult_Helper(nextChar, typeAheadItem, addedFromChildren);
            //System.Diagnostics.Debug.WriteLine($"Adding {typeAheadItem} => {this}");
            return result;
        }

        public bool AddResult_Helper(char? nextChar, TypeAheadItem<R> typeAheadItem, bool addedFromChildren = false)
        {
            //if (TypeAheadItems.Any(x => x.Equals(typeAheadItem)))
            //    return false;
            if (nextChar == null && !addedFromChildren)
                if (!ExactMatches.Contains(typeAheadItem))
                    ExactMatches.Add(typeAheadItem);
            if (nextChar != null && !NextChars.Any(x => x == nextChar))
                NextChars = NextChars.Concat(new char[] { (char)nextChar }).ToArray();
            // If this is less popular than something we have already rejected, then we should reject it too.
            if (MostPopularRejected != null && MostPopularRejected.CompareTo(typeAheadItem) <= 0)
                return false;
            var currentResultsCount = TypeAheadItems.Count();
            // If our buffers are full and this isn't as good as anything we have, then drop it.
            if (currentResultsCount == MaxThreshold && LowestItem.CompareTo(typeAheadItem) < 0)
            {
                if (MostPopularRejected == null || typeAheadItem.CompareTo(MostPopularRejected) < 0)
                    MostPopularRejected = typeAheadItem;
                return false;
            }
            // If we eject an item, update MostPopularRejected if necessary
            if (currentResultsCount == MaxThreshold)
            {
                var dumped = LowestItem;
                if (MostPopularRejected == null || dumped.CompareTo(MostPopularRejected) < 0)
                    MostPopularRejected = dumped;
            }
            // Now, insert the item.
            int i = 0;
            while (i < currentResultsCount && typeAheadItem.CompareTo(TypeAheadItems[i]) > 0)
                i++;
            TypeAheadItems.Insert(i, typeAheadItem);
            if (TypeAheadItems.Count() > MaxThreshold)
                TypeAheadItems = TypeAheadItems.Take(MaxThreshold).ToList();
            return true;
        }


        public MoreResultsRequest<R> RemoveResult(R result)
        {
            MoreResultsRequest<R> request = RemoveResult_Helper(result);
            //System.Diagnostics.Debug.WriteLine($"Removing {result} => {this}");
            return request;
        }

        public MoreResultsRequest<R> RemoveResult_Helper(R result)
        {
            int? index = GetResultIndex(result);
            if (index != null)
            {
                var toRemove = TypeAheadItems[(int)index];
                TypeAheadItems.RemoveAt((int)index);
            }
            ExactMatches.RemoveAll(x => x.SearchResult.Equals(result));
            int resultsCount = TypeAheadItems.Count();
            if (resultsCount == MinThreshold - 1)
            {
                var lowestExistingItem = TypeAheadItems.Any() ? LowestItem : null;
                // first, add exact matches items (if available). These may end up being replaced by child items.
                var itemsToAdd = ExactMatches.Where(x => !TypeAheadItems.Any(y => x.Equals(y))).OrderByDescending(x => x.Popularity).Take(resultsCount);
                if (itemsToAdd.Any())
                    TypeAheadItems = TypeAheadItems.Concat(itemsToAdd).ToList();
                // now, add items from children
                return GetMoreResultsRequest(MaxThreshold - resultsCount + 1, lowestExistingItem); // IMPORTANT: We get one more result than we need. That way, we can set the MostPopularRejected again.
            }
            else
                return null;
        }

        private MoreResultsRequest<R> GetMoreResultsRequest(int maxNumResults, TypeAheadItem<R> lowestExistingItem)
        {
            return new MoreResultsRequest<R>() { NextChars = NextChars, MaxNumResults = maxNumResults, LowestExistingItem = lowestExistingItem };
        }
    }
}
