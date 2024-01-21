using Lazinator.Wrappers;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections;
using Lazinator.Collections.Extensions;
using System;
using System.Collections.Generic;

namespace PerformanceProfiling
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree, true, 100000),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree, true, 1000),
                        new ContainerLevel(ContainerType.LazinatorSortedList, true, 10)
                    });
            var l = factory.CreatePossiblySortedLazinatorListable<WInt>();
            Random r = new Random(0);
            for (int i = 0; i < 150000; i++)
            {
                int n = r.Next(100000);
                l.SortedInsertOrReplace(true, n, Lazinator.Collections.Interfaces.MultivalueLocationOptions.InsertAfterLast, Comparer<WInt>.Default);
            }
        }
    }
}
