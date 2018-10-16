using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections.Avl;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
    public class AvlListTests
    {
        [Fact]
        public void AvlListAddWorks()
        {
            AvlList<WInt> list = new AvlList<WInt>();
            const int numItems = 100;
            for (int i = 0; i < numItems; i++)
                list.Add(i);
            for (int i = 0; i < numItems; i++)
                list[i].WrappedValue.Should().Be(i);
        }

        [Fact]
        public void AvlListInsertAndRemoveWork()
        {
            (int numModifications, double removalProbability)[] phases = new (int numModifications, double removalProbability)[] { (2, 0.25), (4, 0.6), (8, 0.25), (16, 0.6), (32, 0.25), (64, 0.6), };
            Random r = new Random(0);
            const int numRepetitions = 10;
            for (int repetition = 0; repetition < numRepetitions; repetition++)
            {
                int i = 0;
                List<int> list = new List<int>();
                AvlList<WInt> avlList = new AvlList<WInt>();
                foreach (var phase in phases)
                {
                    for (int m = 0; m < phase.numModifications; m++)
                    {
                        ListModificationInstruction instruction = new ListModificationInstruction(r, list, i++, phase.removalProbability);
                        instruction.Execute(list);
                        instruction.Execute(avlList);
                        if (!list.SequenceEqual(avlList.Select(x => x.WrappedValue)))
                            throw new Exception("Match failure.");
                    }
                }
            }
        }

        class ListModificationInstruction
        {
            public bool Remove;
            public int Index;
            public int ValueToAdd;

            public ListModificationInstruction(Random r, IList<int> list, int valueToAdd, double removalProbability)
            {
                if (!list.Any())
                {
                    Remove = false;
                    Index = 0;
                }
                else
                {
                    Remove = r.NextDouble() < removalProbability;
                    Index = r.Next(list.Count());
                    if (!Remove)
                        ValueToAdd = valueToAdd;
                }
            }

            public void Execute(IList<int> list)
            {
                if (Remove)
                    list.RemoveAt(Index);
                else
                    list.Insert(Index, ValueToAdd);
            }

            public void Execute(IList<WInt> list)
            {
                if (Remove)
                    list.RemoveAt(Index);
                else
                    list.Insert(Index, ValueToAdd);
            }
        }


        [Fact]
        public void AvlListIndexOfWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            avlList.IndexOf(0).Should().Be(0);
            avlList.IndexOf(1).Should().Be(1);
            avlList.IndexOf(2).Should().Be(2);
            avlList.IndexOf(3).Should().Be(-1);
        }

        [Fact]
        public void AvlListCopyToWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            WInt[] array = new WInt[6];
            avlList.CopyTo(array, 2);
            array[2].WrappedValue.Should().Be(0);
            array[3].WrappedValue.Should().Be(1);
            array[4].WrappedValue.Should().Be(2);
        }

        [Fact]
        public void AvlListCountWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            avlList.Count.Should().Be(3);
        }

        [Fact]
        public void AvlListClearWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            avlList.Clear();
            avlList.Count.Should().Be(0);
        }
    }
}
