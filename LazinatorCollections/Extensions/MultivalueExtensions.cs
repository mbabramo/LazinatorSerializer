﻿using LazinatorCollections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Extensions
{
    public static class MultivalueExtensions
    {
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