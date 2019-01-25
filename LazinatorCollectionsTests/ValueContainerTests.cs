using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorCollections;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Avl.ValueTree;
using LazinatorCollections.Factories;
using LazinatorCollections.Location;

namespace LazinatorCollectionsTests
{
    public enum ValueContainerToUse
    {
        AvlTree,
        AvlIndexableTree,
        AvlSortedTree,
        AvlSortedIndexableTree,
        AvlSortedListTree,
        LazinatorList,
        LazinatorLinkedList,
        AvlListTreeTinyLazinatorList,
        AvlListTreeSmallLazinatorList,
        AvlListTreeRegularLazinatorList,
        AvlListTreeTinyLinkedList,
        AvlListTreeSmallLinkedList,
        AvlListTreeRegularLinkedList,
        AvlListTreeCompoundedFollowedByLinkedList
    }


    public class ValueContainerTests_WInt : ValueContainerTests<WInt>
    {
        [Theory]
        [InlineData(ValueContainerToUse.AvlTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlIndexableTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedIndexableTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedListTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLazinatorList, false, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLazinatorList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLazinatorList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLinkedList, false, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeCompoundedFollowedByLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorList, false, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlIndexableTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedIndexableTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedListTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLazinatorList, true, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLazinatorList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLazinatorList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLinkedList, true, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLinkedList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLinkedList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeCompoundedFollowedByLinkedList, true, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorList, true, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorLinkedList, true, 50, 50)]
        public void VerifyIntContainer(ValueContainerToUse containerType, bool allowDuplicates, int numRepetitions, int numInstructions) => VerifyValueContainerHelper(containerType, allowDuplicates, numRepetitions, numInstructions);

        [Fact]
        public void VerifyIntContainerDEBUG() => VerifyIntContainer(ValueContainerToUse.LazinatorList, false, 50, 50);


        [Theory]
        [InlineData(ValueContainerToUse.AvlTree)]
        [InlineData(ValueContainerToUse.AvlIndexableTree)]
        [InlineData(ValueContainerToUse.AvlSortedTree)]
        [InlineData(ValueContainerToUse.AvlSortedIndexableTree)]
        [InlineData(ValueContainerToUse.AvlSortedListTree)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLazinatorList)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLazinatorList)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLazinatorList)]
        [InlineData(ValueContainerToUse.AvlListTreeCompoundedFollowedByLinkedList)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLinkedList)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLinkedList)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLinkedList)]
        [InlineData(ValueContainerToUse.LazinatorList)]
        [InlineData(ValueContainerToUse.LazinatorLinkedList)]
        public void ValueContainer_SplitOff(ValueContainerToUse containerType)
        {
            IValueContainer<WInt> container = GetValueContainer(containerType, false);
            const int numItems = 1000;
            for (int i = 0; i < numItems; i++)
                container.InsertOrReplace(i, Comparer<WInt>.Default);
            var splitOff = container.SplitOff();
            (container.Count() + splitOff.Count()).Should().Be(numItems);
            if (splitOff.First() > container.First())
            {
                foreach (var x in splitOff)
                    container.InsertOrReplace(x, Comparer<WInt>.Default);
            }
            else
            {
                foreach (var x in container)
                    splitOff.InsertOrReplace(x, Comparer<WInt>.Default);
                container = splitOff;
            }
            container.Select(x => x.WrappedValue).SequenceEqual(Enumerable.Range(0, numItems)).Should().BeTrue();
        }

        public override WInt GetRandomValue()
        {
            return ran.Next(100);
        }
    }

    public class ValueContainerTests_WString : ValueContainerTests<WString>
    {
        [Theory]
        [InlineData(ValueContainerToUse.AvlTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlIndexableTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedIndexableTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedListTree, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLazinatorList, false, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLazinatorList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLazinatorList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeCompoundedFollowedByLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLinkedList, false, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorList, false, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorLinkedList, false, 50, 50)]
        [InlineData(ValueContainerToUse.AvlTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlIndexableTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedIndexableTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlSortedListTree, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLazinatorList, true, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLazinatorList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeCompoundedFollowedByLinkedList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLazinatorList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeTinyLinkedList, true, 20, 20)]
        [InlineData(ValueContainerToUse.AvlListTreeSmallLinkedList, true, 50, 50)]
        [InlineData(ValueContainerToUse.AvlListTreeRegularLinkedList, true, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorList, true, 50, 50)]
        [InlineData(ValueContainerToUse.LazinatorLinkedList, true, 50, 50)]
        public void VerifyStringContainer(ValueContainerToUse containerType, bool allowDuplicates, int numRepetitions, int numInstructions) => VerifyValueContainerHelper(containerType, allowDuplicates, numRepetitions, numInstructions);

        public override WString GetRandomValue()
        {
            string[] malePetNames = { "Rufus", "Bear", "Dakota", "Fido",
                                "Vanya", "Samuel", "Koani", "Volodya",
                                "Prince", "Yiska" };

            string result = malePetNames[ran.Next(malePetNames.Length)];
            if (ran.Next(5) > 0)
                result += ran.Next(99).ToString();
            return result;
        }
    }

    public abstract class ValueContainerTests<T> where T : ILazinator, IComparable<T>
    {

        public IValueContainer<T> GetValueContainer(ValueContainerToUse containerType, bool allowDuplicates)
        {
            ContainerFactory factory;
            switch (containerType)
            {
                case ValueContainerToUse.AvlTree:
                    return new AvlTree<T>(allowDuplicates, false);
                case ValueContainerToUse.AvlIndexableTree:
                    return new AvlIndexableTree<T>(allowDuplicates, false);
                case ValueContainerToUse.AvlSortedTree:
                    return new AvlSortedTree<T>(allowDuplicates, false);
                case ValueContainerToUse.AvlSortedIndexableTree:
                    return new AvlSortedIndexableTree<T>(allowDuplicates, false);
                case ValueContainerToUse.AvlSortedListTree:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorSortedList, allowDuplicates, 3, false),
                    }
                    );
                    return factory.CreateSortedValueContainer<T>();
                case ValueContainerToUse.AvlListTreeTinyLazinatorList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorList, allowDuplicates, 1, false),
                    }
                    );
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.AvlListTreeSmallLazinatorList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorList, allowDuplicates, 3, false),
                    }
                    );
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.AvlListTreeRegularLazinatorList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorList, allowDuplicates, 5, false),
                    }
                    );
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.AvlListTreeTinyLinkedList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorLinkedList, allowDuplicates, 1, false),
                    }
                    );
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.AvlListTreeSmallLinkedList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorLinkedList, allowDuplicates, 3, false),
                    }
                    );
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.AvlListTreeRegularLinkedList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.LazinatorLinkedList, allowDuplicates, 5, false),
                    }
                    );
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.AvlListTreeCompoundedFollowedByLinkedList:
                    factory = new ContainerFactory(new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, long.MaxValue, false),
                        new ContainerLevel(ContainerType.AvlListTree, allowDuplicates, 10, false),
                        new ContainerLevel(ContainerType.LazinatorLinkedList, allowDuplicates, 2, false),
                    });
                    return factory.CreateValueContainer<T>();
                case ValueContainerToUse.LazinatorList:
                    return new LazinatorList<T>(allowDuplicates);
                case ValueContainerToUse.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>(allowDuplicates);
                default:
                    throw new NotSupportedException();
            }
        }

        public Random ran = new Random(0);
        bool AllowDuplicates;


        public void VerifyValueContainerHelper(ValueContainerToUse containerType, bool allowDuplicates, int numRepetitions, int numInstructions)
        {
            AllowDuplicates = allowDuplicates;
            for (int rep = 0; rep < numRepetitions; rep++)
            {
                List<T> list = new List<T>();
                IValueContainer<T> container = GetValueContainer(containerType, allowDuplicates);
                if (container is IMultivalueContainer<T> multivalue)
                    if (multivalue.AllowDuplicates != AllowDuplicates)
                        throw new Exception("Internal error on AllowDuplicates.");
                for (int i = 0; i < numInstructions; i++)
                {
                    int r = ran.Next(100);
                    RandomInstruction instruction;
                    if (r < 10)
                        instruction = new ChangeValueToSelfInstruction();
                    else if (r < 25)
                        instruction = new GetValueInstruction();
                    else
                        if (r < 75)
                        instruction = new InsertValueInstruction();
                    else
                        instruction = new RemoveInstruction();
                    instruction.Execute(this, container, list);
                    // VerifyEntireList(container, list); 
                }
                VerifyEntireList(container, list);
                if (container is ISortedValueContainer<T>)
                {
                    VerifyEnumerableFromValue(false, container, list);
                    VerifyEnumerableFromValue(true, container, list);
                }
                VerifyEnumerableSkip(false, container, list);
                VerifyEnumerableSkip(true, container, list);
            }
        }

        public void VerifyEntireList(IValueContainer<T> valueContainer, List<T> list)
        {
            var values = valueContainer.AsEnumerable().ToList();
            values.SequenceEqual(list).Should().BeTrue();
        }

        private List<T> GetValues(IValueContainer<T> valueContainer, bool reverse, long skip, bool useEnumerator)
        {
            // This method allows us to test either the AsEnumerable method or the GetEnumerator method. 
            if (useEnumerator)
            {
                List<T> l = new List<T>();
                var enumerator = valueContainer.GetEnumerator(reverse, skip);
                while (enumerator.MoveNext())
                    l.Add(enumerator.Current);
                return l;
            }
            else
                return valueContainer.AsEnumerable(reverse, skip).ToList();
        }

        private List<T> GetValues(IValueContainer<T> valueContainer, bool reverse, T startValue, bool useEnumerator)
        {
            // This method allows us to test either the AsEnumerable method or the GetEnumerator method. 
            if (useEnumerator)
            {
                List<T> l = new List<T>();
                var enumerator = valueContainer.GetEnumerator(reverse, startValue, Comparer<T>.Default);
                while (enumerator.MoveNext())
                    l.Add(enumerator.Current);
                return l;
            }
            else
                return valueContainer.AsEnumerable(reverse, startValue, Comparer<T>.Default).ToList();
        }

        public void VerifyEnumerableFromValue(bool reverse, IValueContainer<T> valueContainer, List<T> list)
        {
            var list2 = list.ToList();
            if (reverse)
                list2.Reverse();
            T randomStartingValue = default;
            if (list2.Any())
            {
                int randomStartingPoint = ran.Next(0, list.Count);
                randomStartingValue = list2[randomStartingPoint];
            }
            var withSkips = list2.SkipWhile(x => reverse ? Comparer<T>.Default.Compare(x, randomStartingValue) > 0 : Comparer<T>.Default.Compare(x, randomStartingValue) < 0).ToList();
            var values = GetValues(valueContainer, reverse, randomStartingValue, false);
            values.SequenceEqual(withSkips).Should().BeTrue();
            values = GetValues(valueContainer, reverse, randomStartingValue, true);
            values.SequenceEqual(withSkips).Should().BeTrue();
        }
        

        public void VerifyEnumerableSkip(bool reverse, IValueContainer<T> valueContainer, List<T> list)
        {
            var list2 = list.ToList();
            if (reverse)
                list2.Reverse();
            int numToSkip = ran.Next(0, list.Count + 1);
            var withSkips = list2.Skip(numToSkip).ToList();
            var values = GetValues(valueContainer, reverse, numToSkip, false);
            values.SequenceEqual(withSkips).Should().BeTrue();
            values = GetValues(valueContainer, reverse, numToSkip, true);
            values.SequenceEqual(withSkips).Should().BeTrue();
        }

        public (T item, int firstIndex, int lastIndex)? GetRandomItem(List<T> list)
        {
            if (!list.Any())
                return null;
            int index = ran.Next(0, list.Count());
            T item = list[index];
            int firstIndex, lastIndex;
            GetIndexRange(list, index, out firstIndex, out lastIndex);
            return (item, firstIndex, lastIndex);
        }

        public static void GetIndexRange(List<T> list, int index, out int firstIndex, out int lastIndex)
        {
            T item = list[index];
            firstIndex = index;
            lastIndex = index;
            while (firstIndex > 0 && list[firstIndex - 1].Equals(item))
                firstIndex--;
            while (lastIndex < list.Count() - 1 && list[lastIndex + 1].Equals(item))
                lastIndex++;
        }

        public MultivalueLocationOptions ChooseMultivalueInsertOption()
        {
            int i = ran.Next(0, 5);
            switch (i)
            {
                case 0:
                    return MultivalueLocationOptions.Any;
                case 1:
                    return MultivalueLocationOptions.First;
                case 2:
                    return MultivalueLocationOptions.Last;
                case 3:
                    return MultivalueLocationOptions.InsertBeforeFirst;
                case 4:
                    return MultivalueLocationOptions.InsertAfterLast;
                default:
                    throw new NotSupportedException();
            }
        }

        public MultivalueLocationOptions ChooseMultivalueDeleteOption()
        {
            int i = ran.Next(0, 3);
            switch (i)
            {
                case 0:
                    return MultivalueLocationOptions.Any;
                case 1:
                    return MultivalueLocationOptions.First;
                case 2:
                    return MultivalueLocationOptions.Last;
                default:
                    throw new NotSupportedException();
            }
        }

        public (int index, bool insertedNotReplaced) InsertOrReplaceItem(List<T> list, T item, bool sorted, MultivalueLocationOptions whichOne)
        {
            if (sorted)
            {
                int index = list.BinarySearch(item);
                bool found = index >= 0;
                if (!found)
                    index = ~index;
                bool replace = false;
                if (found)
                {
                    GetIndexRange(list, index, out int firstIndex, out int lastIndex);
                    switch (whichOne)
                    {
                        case MultivalueLocationOptions.Any:
                            replace = true;
                            break;
                        case MultivalueLocationOptions.First:
                            index = firstIndex;
                            replace = true;
                            break;
                        case MultivalueLocationOptions.Last:
                            index = lastIndex;
                            replace = true;
                            break;
                        case MultivalueLocationOptions.InsertBeforeFirst:
                            index = firstIndex;
                            replace = false;
                            break;
                        case MultivalueLocationOptions.InsertAfterLast:
                            index = lastIndex + 1;
                            replace = false;
                            break;
                    }
                }
                if (replace)
                {
                    list[index] = item;
                    return (index, false);
                }
                else
                {
                    list.Insert(index, item);
                    return (index, true);
                }
            }
            else
            {
                if (whichOne == MultivalueLocationOptions.InsertAfterLast || whichOne == MultivalueLocationOptions.InsertBeforeFirst || list.Count == 0)
                {
                    int index = ran.Next(0, list.Count + 1);
                    list.Insert(index, item);
                    return (index, true);
                }
                else
                {
                    int index = ran.Next(0, list.Count);
                    list[index] = item;
                    return (index, false);
                }
            }
        }

        public abstract T GetRandomValue();

        public abstract class RandomInstruction
        {
            protected bool ContainerIsSorted;
            protected ISortedValueContainer<T> SortedContainer;

            public virtual void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                EstablishSorted(container);
                switch (container)
                {
                    case AvlSortedIndexableTree<T> sortedIndexableContainer when sortedIndexableContainer.AllowDuplicates == true:
                        Execute_SortedIndexableMultivalue(testClass, sortedIndexableContainer, list);
                        break;
                    case AvlSortedTree<T> sortedContainer when sortedContainer.AllowDuplicates == true:
                        Execute_SortedMultivalue(testClass, sortedContainer, list);
                        break;
                    case IIndexableMultivalueContainer<T> indexableContainer when indexableContainer.AllowDuplicates == true:
                        Execute_IndexableMultivalue(testClass, indexableContainer, list);
                        break;
                    case IMultivalueContainer<T> basicContainer when basicContainer.AllowDuplicates == true:
                        Execute_Multivalue(testClass, basicContainer, list);
                        break;
                    case AvlSortedIndexableTree<T> sortedIndexableContainer when sortedIndexableContainer.AllowDuplicates == false:
                        Execute_SortedIndexable(testClass, sortedIndexableContainer, list);
                        break;
                    case AvlSortedTree<T> sortedContainer when sortedContainer.AllowDuplicates == false:
                        Execute_Sorted(testClass, sortedContainer, list);
                        break;
                    case IIndexableMultivalueContainer<T> indexableContainer when indexableContainer.AllowDuplicates == false:
                        Execute_Indexable(testClass, indexableContainer, list);
                        break;
                    case IValueContainer<T> basicContainer:
                        Execute_Value(testClass, basicContainer, list);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }


            protected void EstablishSorted(IValueContainer<T> container)
            {
                SortedContainer = container as ISortedValueContainer<T>;
                ContainerIsSorted = container is ISortedValueContainer<T>;
            }

            public abstract void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list);
            public abstract void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableValueContainer<T> container, List<T> list);
            public abstract void Execute_Sorted(ValueContainerTests<T> testClass, ISortedValueContainer<T> container, List<T> list);
            public abstract void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list);
            public abstract void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list);
            public abstract void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list);
            public abstract void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list);
            public abstract void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list);
            public IComparer<T> C => Comparer<T>.Default;
            public bool Eq(T item, T other)
            {
                return EqualityComparer<T>.Default.Equals(item, other);
            }
            public void AssertEqual(T item, T other)
            {
                Eq(item, other).Should().BeTrue();
            }
            public void AssertNotEqual(T item, T other)
            {
                Eq(item, other).Should().BeFalse();
            }
            public void VerifyExpectedIndex(MultivalueLocationOptions whichOne, (T item, int firstIndex, int lastIndex) listResult, long indexableContainerResult)
            {
                if (whichOne == MultivalueLocationOptions.First || whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    indexableContainerResult.Should().Be(listResult.firstIndex);
                else if (whichOne == MultivalueLocationOptions.Last)
                    indexableContainerResult.Should().Be(listResult.lastIndex);
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    indexableContainerResult.Should().Be(listResult.lastIndex + 1);
                else if (whichOne == MultivalueLocationOptions.Any)
                {
                    indexableContainerResult.Should().BeGreaterOrEqualTo(listResult.firstIndex);
                    indexableContainerResult.Should().BeLessOrEqualTo(listResult.lastIndex);
                }
            }
        }

        public class GetValueInstruction : RandomInstruction
        {
            public override void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableValueContainer<T> container, List<T> list)
            {
                Execute_IndexableHelper(testClass, container, list, MultivalueLocationOptions.Any);
            }

            private void Execute_IndexableHelper(ValueContainerTests<T> testClass, IIndexableValueContainer<T> container, List<T> list, MultivalueLocationOptions whichOne)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    if (ContainerIsSorted)
                    {
                        var findResult = container.FindIndex(default, C);
                        findResult.index.Should().Be(0);
                        findResult.exists.Should().BeFalse();
                    }
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        T getAtResult = container.GetAtIndex(i);
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list)
            {
                foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last }) // other options are undefined
                {
                    Execute_IndexableHelper(testClass, container, list, whichOne);
                }
            }

            public override void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    var getValueResult = container.GetValue(default, MultivalueLocationOptions.Any, C, out T getValueMatch);
                    getValueResult.Should().BeFalse();
                    AssertEqual(getValueMatch, default(T));
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last })
                    {
                        var getValueResult = container.GetValue(listResult.item, whichOne, C, out T getValueMatch);
                        getValueResult.Should().BeTrue();
                        AssertEqual(getValueMatch, listResult.item);
                    }
                    long count = container.Count(listResult.item, Comparer<T>.Default);
                    count.Should().Be(listResult.lastIndex - listResult.firstIndex + 1);
                }
            }

            public override void Execute_Sorted(ValueContainerTests<T> testClass, ISortedValueContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    bool containsResult = container.Contains(default);
                    containsResult.Should().BeFalse();
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    bool containsResult = container.Contains(listResult.item);
                    containsResult.Should().BeTrue();
                }
            }

            public override void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_Indexable(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    var findResult = container.FindIndex(listResult.item);
                    VerifyExpectedIndex(MultivalueLocationOptions.Any, listResult, findResult.index);
                    findResult.exists.Should().BeTrue();
                    for (int i = listResult.firstIndex; i <= listResult.lastIndex; i++)
                    {
                        T getAtResult = container.GetAtIndex(i);
                        AssertEqual(getAtResult, listResult.item);
                    }
                }
            }

            public override void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    Execute_IndexableMultivalue(testClass, container, list);
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    foreach (MultivalueLocationOptions whichOne in new MultivalueLocationOptions[] { MultivalueLocationOptions.First, MultivalueLocationOptions.Any, MultivalueLocationOptions.Last }) // other options are undefined
                    {
                        var findResult = container.FindIndex(listResult.item, whichOne);
                        VerifyExpectedIndex(whichOne, listResult, findResult.index);
                        findResult.exists.Should().BeTrue();
                    }
                }
            }

            public override void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list)
            {
                Execute_Multivalue(testClass, container, list);
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull != null)
                {
                    var listResult = listResultOrNull.Value;
                    long count = container.Count(listResult.item, Comparer<T>.Default);
                    count.Should().Be(listResult.lastIndex - listResult.firstIndex + 1);
                }
            }

            public override void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                var listResultOrNull = testClass.GetRandomItem(list);
                if (listResultOrNull == null)
                {
                    bool present = container.GetValue(default, C, out T match);
                    present.Should().BeFalse();
                    AssertEqual(match, default(T));
                }
                else
                {
                    var listResult = listResultOrNull.Value;
                    bool present = container.GetValue(listResult.item, C, out T match);
                    present.Should().BeTrue();
                    AssertEqual(match, listResultOrNull.Value.item);
                }
            }
        }

        public class ChangeValueToSelfInstruction : RandomInstruction
        {
            MultivalueLocationOptions WhichOne;

            public override void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                EstablishSorted(container);
                WhichOne = testClass.ran.Next(2) == 0 ? MultivalueLocationOptions.First : MultivalueLocationOptions.Last;
                if (!list.Any())
                    return;
                int index = testClass.ran.Next(list.Count);
                T value = list[index];
                IContainerLocation location;
                bool found;
                if ((container is IMultivalueContainer<T> && !ContainerIsSorted) || testClass.ran.Next(5) == 0)
                {
                    // if list isn't sorted, we can't use FindContainerLocation. But this gives us an opportunity to test FirstLocation / LastLocation / GetNextLocation / GetPreviousLocation instead
                    if (testClass.ran.Next(2) == 0)
                    {
                        location = container.FirstLocation();
                        for (int i = 0; i < index; i++)
                        {
                            var DEBUG = container.GetAt(location);
                            System.Diagnostics.Debug.WriteLine(DEBUG);
                            location = location.GetNextLocation();
                        }
                    }
                    else
                    {
                        location = container.LastLocation();
                        for (int i = list.Count() - 1; i > index; i--)
                            location = location.GetPreviousLocation();
                    }
                    found = true;
                }
                else if (container is IMultivalueContainer<T> multivalueContainer)
                {
                     (location, found) = multivalueContainer.FindContainerLocation(value, WhichOne, Comparer<T>.Default);
                }
                else
                {
                    (location, found) = container.FindContainerLocation(value, Comparer<T>.Default);
                }

                found.Should().BeTrue();
                container.GetAt(location).Equals(value).Should().BeTrue();
                container.SetAt(location, value); // should have no effect
            }

            // Since logic is handled above, there is no need to implement abstract methods.

            public override void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableValueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_Sorted(ValueContainerTests<T> testClass, ISortedValueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }

            public override void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                throw new NotImplementedException();
            }
        }

        public class InsertValueInstruction : RandomInstruction
        {

            T Item;
            MultivalueLocationOptions WhichOne;
            int Index;
            bool InsertedNotReplaced;

            public override void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                Item = testClass.GetRandomValue();
                WhichOne = testClass.AllowDuplicates ? testClass.ChooseMultivalueInsertOption() : MultivalueLocationOptions.Any; // if not multivalue, just replace the item
                EstablishSorted(container);
                // If we are using the base container type, then we can only add items with a comparer, so we treat it as a sorted container.
                (Index, InsertedNotReplaced) = testClass.InsertOrReplaceItem(list, Item, ContainerIsSorted || !(container is IIndexableValueContainer<T>), WhichOne);
                if (InsertedNotReplaced && testClass.ran.Next(2) == 0)
                {
                    // test InsertAt with location
                    (IContainerLocation location, bool found) findResult;
                    if (!ContainerIsSorted && container is IIndexableValueContainer<T>)
                    {
                        findResult.location = new IndexLocation(Index, list.Count());
                    }
                    else
                    {

                        MultivalueLocationOptions whichOneModified = WhichOne;
                        if (WhichOne == MultivalueLocationOptions.InsertAfterLast)
                            whichOneModified = MultivalueLocationOptions.Last;
                        else if (WhichOne == MultivalueLocationOptions.InsertBeforeFirst)
                            whichOneModified = MultivalueLocationOptions.First;
                        if (container is IMultivalueContainer<T> multivalueContainer)
                            findResult = multivalueContainer.FindContainerLocation(Item, whichOneModified, Comparer<T>.Default);
                        else
                            findResult = container.FindContainerLocation(Item, Comparer<T>.Default);
                    }
                    container.InsertAt(findResult.location, Item);
                }
                else
                    base.Execute(testClass, container, list);
            }

            public override void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableValueContainer<T> container, List<T> list)
            {
                if (InsertedNotReplaced)
                    container.InsertAtIndex(Index, Item);
                else
                    container.SetAtIndex(Index, Item);
            }

            public override void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list)
            {
                Execute_Indexable(testClass, container, list);
            }

            public override void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list)
            {
                var result = container.InsertOrReplace(Item, WhichOne, C);
                result.insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_Sorted(ValueContainerTests<T> testClass, ISortedValueContainer<T> container, List<T> list)
            {
                var result = container.InsertOrReplace(Item);
                result.insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list)
            {
                (IContainerLocation location, bool insertedNotReplaced) = container.InsertOrReplace(Item);
                ((IndexLocation)location).Index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list)
            {
                (IContainerLocation location, bool insertedNotReplaced) = container.InsertOrReplace(Item, WhichOne);
                // if WhichOne == Any, then exact location is undefined, so we don't verify it. The behavior may be different from our list implementation because which one is selected may be based on the ordering of the binary tree. The list binary search algorithm always starts from the middle element, while a tree search will start from the top of the tree, which may not be the exact middle element.
                if (WhichOne != MultivalueLocationOptions.Any)
                    ((IndexLocation)location).Index.Should().Be(Index);
                insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list)
            {
                var result = container.InsertOrReplace(Item, WhichOne);
                result.insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }

            public override void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                var result = container.InsertOrReplace(Item, C);
                result.insertedNotReplaced.Should().Be(InsertedNotReplaced);
            }
        }

        public class RemoveInstruction : RandomInstruction
        {
            T ValueToTryToRemove;
            bool ValueExisted;
            int IndexBeforeRemove;
            MultivalueLocationOptions WhichOne;
            bool RemoveAll;
            int FirstIndex, LastIndex;

            public override void Execute(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                EstablishSorted(container);
                if (testClass.AllowDuplicates)
                {
                    if (ContainerIsSorted && testClass.ran.Next(0, 5) == 0)
                    {
                        RemoveAll = true; // overrides remaining settings
                        WhichOne = MultivalueLocationOptions.InsertAfterLast; // invalid -- but we won't use it in a call; we use something invalid to ensure that if we do, we'll get an error
                    }
                    else
                        WhichOne = testClass.ChooseMultivalueDeleteOption();
                }
                else
                    WhichOne = MultivalueLocationOptions.Any;
                PlanRemoval(testClass, list);
                if (ValueExisted)
                {
                    if (RemoveAll)
                    {
                        for (int i = FirstIndex; i <= LastIndex; i++)
                            list.RemoveAt(FirstIndex);
                    }
                    else switch (WhichOne)
                    {
                        case MultivalueLocationOptions.Any:
                            list.RemoveAt(IndexBeforeRemove);
                            break;
                        case MultivalueLocationOptions.First:
                            list.RemoveAt(FirstIndex);
                            break;
                        case MultivalueLocationOptions.Last:
                            list.RemoveAt(LastIndex);
                            break;
                        default: throw new NotSupportedException();
                    }
                }
                if (ValueExisted && !RemoveAll && testClass.ran.Next(2) == 0)
                {
                    // test RemoveAt with location
                    (IContainerLocation location, bool found) findResult;
                    if (!ContainerIsSorted && container is IIndexableValueContainer<T>)
                    {
                        findResult.location = new IndexLocation(IndexBeforeRemove, list.Count());
                    }
                    else
                    {
                        findResult = container.FindContainerLocation(ValueToTryToRemove, Comparer<T>.Default);
                        findResult.found.Should().BeTrue();
                    }
                    container.RemoveAt(findResult.location);
                }
                else
                    base.Execute(testClass, container, list);
            }

            public void PlanRemoval(ValueContainerTests<T> testClass, List<T> list)
            {
                if (!ContainerIsSorted)
                {
                    // Removing by index
                    if (list.Any())
                    {
                        ChooseItemInListToRemove(testClass, list);
                    }
                    else
                    {
                        ValueExisted = false;
                        ValueToTryToRemove = default;
                        IndexBeforeRemove = -1;
                    }
                    return;
                }
                if (!list.Any() || testClass.ran.Next(5) == 0)
                { // try to find something NOT in list to try (and fail) to remove
                    const int maxTriesToFindNotIncludedItem = 10;
                    for (int i = 0; i < maxTriesToFindNotIncludedItem; i++)
                    {
                        T randomValue = testClass.GetRandomValue();
                        int index = list.BinarySearch(randomValue, Comparer<T>.Default);
                        if (index < 0)
                        {
                            ValueToTryToRemove = randomValue;
                            ValueExisted = false;
                            IndexBeforeRemove = -1;
                            return;
                        }
                        else if (i == maxTriesToFindNotIncludedItem - 1)
                        { // We'll go with something in the list
                            ValueToTryToRemove = randomValue;
                            IndexBeforeRemove = index;
                            ValueExisted = true;
                            ValueContainerTests<T>.GetIndexRange(list, IndexBeforeRemove, out FirstIndex, out LastIndex);
                            return;
                        }
                    }
                }
                else
                {
                    ChooseItemInListToRemove(testClass, list);
                }
            }

            private void ChooseItemInListToRemove(ValueContainerTests<T> testClass, List<T> list)
            {
                ValueExisted = true;
                IndexBeforeRemove = testClass.ran.Next(list.Count);
                ValueToTryToRemove = list[IndexBeforeRemove];
                ValueContainerTests<T>.GetIndexRange(list, IndexBeforeRemove, out FirstIndex, out LastIndex);
            }

            public override void Execute_Indexable(ValueContainerTests<T> testClass, IIndexableValueContainer<T> container, List<T> list)
            {
                if (IndexBeforeRemove != -1)
                {
                    container.RemoveAt(IndexBeforeRemove);
                }
            }

            public override void Execute_IndexableMultivalue(ValueContainerTests<T> testClass, IIndexableMultivalueContainer<T> container, List<T> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove, Comparer<T>.Default);
                else
                    Execute_Indexable(testClass, container, list);
            }

            public override void Execute_Multivalue(ValueContainerTests<T> testClass, IMultivalueContainer<T> container, List<T> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove, Comparer<T>.Default);
                else
                {
                    bool result = container.TryRemove(ValueToTryToRemove, WhichOne, Comparer<T>.Default);
                    VerifySuccess(result);
                }
            }

            public override void Execute_Sorted(ValueContainerTests<T> testClass, ISortedValueContainer<T> container, List<T> list)
            {
                bool result = container.TryRemove(ValueToTryToRemove);
                VerifySuccess(result);
            }

            private void VerifySuccess(bool result)
            {
                if (IndexBeforeRemove == -1)
                    result.Should().BeFalse();
                else
                    result.Should().BeTrue();
            }

            public override void Execute_SortedIndexable(ValueContainerTests<T> testClass, ISortedIndexableContainer<T> container, List<T> list)
            {
                if (testClass.ran.Next(2) == 0)
                    Execute_Sorted(testClass, container, list);
                else
                    Execute_Indexable(testClass, container, list);
            }

            public override void Execute_SortedIndexableMultivalue(ValueContainerTests<T> testClass, ISortedIndexableMultivalueContainer<T> container, List<T> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove);
                else
                {
                    if (testClass.ran.Next(2) == 0)
                        Execute_SortedMultivalue(testClass, container, list);
                    else
                        Execute_Indexable(testClass, container, list);
                }
            }

            public override void Execute_SortedMultivalue(ValueContainerTests<T> testClass, ISortedMultivalueContainer<T> container, List<T> list)
            {
                if (RemoveAll)
                    container.TryRemoveAll(ValueToTryToRemove);
                else
                {
                    bool result = container.TryRemove(ValueToTryToRemove, WhichOne);
                    VerifySuccess(result);
                }
            }

            public override void Execute_Value(ValueContainerTests<T> testClass, IValueContainer<T> container, List<T> list)
            {
                bool result = container.TryRemove(ValueToTryToRemove, Comparer<T>.Default);
                if (IndexBeforeRemove == -1)
                    result.Should().BeFalse();
                else
                    result.Should().BeTrue();
            }
        }
    }
}
