using LazinatorCollections.Interfaces;
using Lazinator.Core;
using System.Collections.Generic;

namespace LazinatorCollections.Extensions
{
    /// <summary>
    /// Utility methods for multivalue containers
    /// </summary>
    public static class MultivalueExtensions
    {
        /// <summary>
        /// Obtain an enumerable from a sorted multivalue container, starting at a specified value and moving either forward or backward
        /// </summary>
        public static IEnumerable<T> MultivalueAsEnumerable<C, T>(this IMultivalueContainer<T> container, bool reverse, T startValue, IComparer<T> comparer) where C : IMultivalueContainer<T> where T : ILazinator
        {
            if (!container.Any())
                yield break;
            if (reverse)
            {
                var result = container.FindContainerLocation(startValue, MultivalueLocationOptions.Last, comparer);
                while (!result.location.IsBeforeContainer)
                {
                    yield return container.GetAt(result.location);
                    result.location = result.location.GetPreviousLocation();
                }
            }
            else
            {
                var result = container.FindContainerLocation(startValue, MultivalueLocationOptions.First, comparer);
                while (!result.location.IsAfterContainer)
                {
                    yield return container.GetAt(result.location);
                    result.location = result.location.GetNextLocation();
                }
            }
        }
    }
}
