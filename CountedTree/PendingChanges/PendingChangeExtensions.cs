using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.PendingChanges
{
    public static class PendingChangeArrayExtensions
    {
        /// <summary>
        /// Orders pending changes from a pending changes set by ID, and then with deleted items before non-deleted items. Since we do not keep in a pending changes set an addition followed by an addition, there should be no more than two entries per ID, and this will reflect the actual order by date of change. This is used to undo the effects of OrderByKey, when concatenating different sets of pending changes. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PendingChange<TKey>[] OrderByDateOfChange<TKey>(this IEnumerable<PendingChange<TKey>> input) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            return input
                .OrderBy(x => x.Item.ID)
                .ThenBy(x => !x.Delete) // if not added, that is first. This way, delete at an ID will be before add. Since we only have two items 
                .ThenBy(x => x.Item.Key) // finally, we might be deleting multiple versions of the same item (different values, same ID)
                .ToArray();
        }

        public static PendingChange<TKey>[] OrderByKey<TKey>(this IEnumerable<PendingChange<TKey>> input) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            return input
                .OrderBy(x => x.Item.Key) // note that we order ONLY by Key so that when simplifying, we will keep stable the order of the changes.
                .ToArray();
        }

        public static bool ContainsMoves<TKey>(this PendingChange<TKey>[] input, bool orderedByDateOfChange) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            var ordered = orderedByDateOfChange ? input : input.OrderByDateOfChange();
            for (int i = 1; i < ordered.Length; i++)
                if (ordered[i - 1].Item.ID == ordered[i].Item.ID && ordered[i - 1].Delete != ordered[i].Delete)
                    return true;
            return false;
        }

        public static PendingChange<TKey>[] Simplify<TKey>(this IEnumerable<PendingChange<TKey>> input) where TKey : struct, ILazinator,
              IComparable,
              IComparable<TKey>,
              IEquatable<TKey>
        {
            var simplified = SimplifyHelper(input).ToList();
            var ordered = simplified.OrderByKey().ToArray();
            return ordered;
        }

        private static IEnumerable<PendingChange<TKey>> SimplifyHelper<TKey>(this IEnumerable<PendingChange<TKey>> input) where TKey : struct, ILazinator,
              IComparable,
              IComparable<TKey>,
              IEquatable<TKey>
        {
            Dictionary<uint, PendingChangeStatus<TKey>> changeStatusDict = new Dictionary<uint, PendingChangeStatus<TKey>>();
            foreach (var pendingChange in input)
            {
                if (changeStatusDict.ContainsKey(pendingChange.Item.ID))
                    changeStatusDict[pendingChange.Item.ID].Process(pendingChange.Item.Key, pendingChange.Delete);
                else
                    changeStatusDict[pendingChange.Item.ID] = new PendingChangeStatus<TKey>(pendingChange.Item.Key, pendingChange.Delete);
            }
            foreach (var changeStatus in changeStatusDict.OrderBy(x => x.Key))
                foreach (var pcOut in changeStatus.Value.GetChanges(changeStatus.Key))
                    yield return pcOut;
        }

        
        
    }
}
