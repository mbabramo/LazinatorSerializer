using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountedTree.TypeAhead;
using Xunit;
using FluentAssertions;
using Utility;
using Lazinator.Wrappers;
using System.Diagnostics;

namespace CountedTree.Tests
{
    public class TypeAheadTests
    {
        [Fact]
        public Task TypeAheadNodeAddResultWorks()
        {
            int minThreshold = 2;
            int maxThreshold = 4;
            TypeAheadNode<WString> node;
            List<Tuple<char?, string, float>> items;
            PopulateNode("noname", minThreshold, maxThreshold, out node, out items);
            var expectedResults = items.OrderByDescending(x => x.Item3).Take(maxThreshold).Select(x => x.Item2);
            node.GetResults(maxThreshold).Select(x => x.WrappedValue).Should().BeEquivalentTo(expectedResults);
            return Task.CompletedTask;
        }

        [Fact]
        public Task TypeAheadNodeRemoveResultWorks()
        {
            int minThreshold = 2;
            int maxThreshold = 4;
            TypeAheadNode<WString> node;
            List<Tuple<char?, string, float>> items;
            PopulateNode("noname", minThreshold, maxThreshold, out node, out items);
            int numResultsLeft = 0;
            foreach (var item in items)
            {
                MoreResultsRequest<WString> m = node.RemoveResult(item.Item2);
                numResultsLeft = node.GetResults(maxThreshold).Count();
                if (numResultsLeft == minThreshold - 1)
                {
                    m.MaxNumResults.Should().Be(maxThreshold - numResultsLeft + 1); // we always request 1 more than we need, so that we can know the highest rejected value
                    m.NextChars.Should().BeEquivalentTo(items.Where(x => x.Item1 != null).Select(x => ((char)x.Item1)).Distinct().ToArray());
                }
            }
            numResultsLeft.Should().Be(0);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task TypeAheadWorks()
        {
           // await TypeAheadWorks(1000, 850, 300, 100);
            for (int numToAdd = 1000; numToAdd <= 1000; numToAdd++) // go from 0 to a high number to find where a problem emerges
            {
                for (int i = 0; i < 1; i++)
                {
                    RandomGenerator.Reset(i * 1000000 + numToAdd);
                    int numToDelete = (int)(numToAdd * RandomGenerator.GetRandom());
                    int secondBatchToAdd = (int)(numToAdd * RandomGenerator.GetRandom() * 1.1);
                    int numPopularityToChange = (int)(numToAdd * RandomGenerator.GetRandom());
                    bool focusOnSpecificCase = false;
                    bool narrowDownToSpecificCase = false;
                    if (focusOnSpecificCase)
                    {
                        numToDelete = 18;
                        secondBatchToAdd = 11; 
                        numPopularityToChange = 3;
                    }
                    if (narrowDownToSpecificCase)
                    {
                        for (int j = 0; j <= numPopularityToChange; j++)
                        {
                            RandomGenerator.Reset(i * 1000 + numToAdd);
                            await TypeAheadWorks_Options(numToAdd, numToDelete, secondBatchToAdd, j);
                        }
                    }
                    RandomGenerator.Reset(i * 1000 + numToAdd);
                    await TypeAheadWorks_Options(numToAdd, numToDelete, secondBatchToAdd, numPopularityToChange);
                }
            }
        }

        private async Task TypeAheadWorks_Options(int numToStartWith, int numToDelete, int secondBatchToAdd, int numPopularityToChange)
        {
            InMemoryTypeAheadStorage<WInt32> executor = new InMemoryTypeAheadStorage<WInt32>();
            executor.MinThreshold = 10;
            executor.MaxThreshold = 15;
            TypeAheadCoordinator<WInt32> typeAhead = new TypeAheadCoordinator<WInt32>(executor);
            Guid context = RandomGenerator.GetGuid();
            string printResultsForPhrase = null; 
            bool printSteps = printResultsForPhrase != null;
            async Task PrintResults()
            {
                if (printResultsForPhrase != null)
                {
                    if (printSteps)
                        Debug.WriteLine("");
                    var actualResults = await typeAhead.GetTypeAheadItems(context, printResultsForPhrase, executor.MaxThreshold);
                    if (printSteps)
                    {
                        foreach (var actualResult in actualResults)
                            Debug.WriteLine(actualResult);
                        Debug.WriteLine("");
                    }
                }
            }
            List<Tuple<string, int, float>> searchPhrasesStored = GetRandomItems(0, numToStartWith, 15);
            var searchPhrasesAdded = searchPhrasesStored.ToList(); // make another copy of everything we initially add 

            List<Tuple<string, int, float>> GetSearchPhrasesResulting()
            {
                // now figure out what we have -- note that we can still find indices by looking in searchPhrasesAdded
                var searchPhrasesResulting = searchPhrasesStored.ToList();
                searchPhrasesResulting = searchPhrasesResulting.OrderBy(x => x.Item3).ToList();
                return searchPhrasesResulting;
            }

            async Task VerifyResultsForPhrase(string typedInPhrase)
            {
                if (typedInPhrase == null)
                    return;
                var actualResults = await typeAhead.GetTypeAheadItems(context, typedInPhrase, executor.MaxThreshold);
                // We need to load the same number of expected results, but also make sure that there isn't anything missing.
                int numToLoadForExpected = Math.Max(actualResults.Count(), executor.MinThreshold); // will load more than actual results up to the minimum threshold, but only if those actually exist
                var expectedResults = GetExpectedResults(GetSearchPhrasesResulting(), typedInPhrase, numToLoadForExpected);
                actualResults.Should().Equal(expectedResults);
            }
            async Task VerifyResultsForRandomPhrases()
            {
                // Now look at some short phrases and see if we have the right items
                var typedInPhrases = GetRandomItems(0, 20, 4).Select(x => x.Item1).Distinct().ToList();
                typedInPhrases.Add("x"); // nothing starts with this
                foreach (var typedInPhrase in typedInPhrases)
                {
                    await VerifyResultsForPhrase(typedInPhrase);
                }
            }
            foreach (var item in searchPhrasesStored)
            {
                var itemToAdd = new TypeAheadItem<WInt32>(item.Item2, item.Item3);
                await typeAhead.AddResult(context, item.Item1, itemToAdd);
                if (printSteps)
                    Debug.WriteLine($"Adding {item.Item1}: {itemToAdd}");
            }
            await PrintResults();
            // now delete some
            for (int i = 0; i < numToDelete; i++)
            {
                var searchPhraseToDelete = RandomGenerator.PickRandom(searchPhrasesStored);
                searchPhrasesStored.Remove(searchPhraseToDelete);
                await typeAhead.RemoveResult(context, searchPhraseToDelete.Item1, searchPhraseToDelete.Item2);
                if (printSteps)
                    Debug.WriteLine($"{i}: Removing {searchPhraseToDelete}: {searchPhraseToDelete.Item2}");
                await PrintResults();
                await VerifyResultsForPhrase(printResultsForPhrase); 
            }
            await PrintResults();
            // and add a bunch more
            var moreSearchPhrases = GetRandomItems(numToStartWith, secondBatchToAdd, 15);
            searchPhrasesStored.AddRange(moreSearchPhrases);
            searchPhrasesAdded.AddRange(moreSearchPhrases);
            foreach (var item in moreSearchPhrases)
            {
                var itemToAdd = new TypeAheadItem<WInt32>(item.Item2, item.Item3);
                await typeAhead.AddResult(context, item.Item1, itemToAdd);
                if (printSteps)
                    Debug.WriteLine($"Adding {item.Item1}: {itemToAdd}");
            }
            await PrintResults();
            // and change popularity on a bunch of those that are left
            for (int i = 0; i < numPopularityToChange; i++)
            {
                var searchPhraseToChange
                    = RandomGenerator.PickRandom(searchPhrasesStored);
                var revisedPopularity = RandomGenerator.GetRandom(0.0F, 100.0F);
                if (printSteps)
                    Debug.WriteLine($"{i}: Changing {searchPhraseToChange.Item1} <{searchPhraseToChange.Item2}, {searchPhraseToChange.Item3} to popularity {revisedPopularity}");
                var replacement = new Tuple<string, int, float>(searchPhraseToChange.Item1, searchPhraseToChange.Item2, revisedPopularity);
                await typeAhead.ChangePopularity(context, replacement.Item1, new TypeAheadItem<WInt32>(replacement.Item2, replacement.Item3));
                if (printSteps)
                    await PrintResults();
                searchPhrasesStored.Remove(searchPhraseToChange);
                searchPhrasesStored.Add(replacement);
                await VerifyResultsForPhrase(printResultsForPhrase);
            }
            await PrintResults();
            await VerifyResultsForRandomPhrases();
        }

        private static void PopulateNode(string stringToHere, int minThreshold, int maxThreshold, out TypeAheadNode<WString> node, out List<Tuple<char?, string, float>> items)
        {
            node = new TypeAheadNode<WString>(stringToHere, minThreshold, maxThreshold);
            items = new List<Tuple<char?, string, float>>()
            {
                new Tuple<char?, string, float>('c', "result 1", 5.0F),
                new Tuple<char?, string, float>('d', "result 2", 2.0F),
                new Tuple<char?, string, float>('e', "result 3", 3.0F),
                new Tuple<char?, string, float>('c', "result 4", 4.0F),
                new Tuple<char?, string, float>(null, "result 5", 0.0F),
                new Tuple<char?, string, float>('g', "result 6", 6.0F),
                new Tuple<char?, string, float>('h', "result 7", 1.0F),
            };
            foreach (var item in items)
                node.AddResult(item.Item1, new TypeAheadItem<WString>(item.Item2, item.Item3));
        }

        private static List<TypeAheadItem<WInt32>> GetExpectedResults(List<Tuple<string, int, float>> searchPhrasesStored, string typedInPhrase, int numToGet)
        {
            return searchPhrasesStored
                .Where(x => x.Item1.StartsWith(typedInPhrase))
                .OrderByDescending(x => x.Item3)
                .Select(x => new TypeAheadItem<WInt32>(x.Item2, x.Item3))
                .Take(numToGet)
                .ToList();
        }



        private static List<Tuple<string, int, float>> GetRandomItems(int min, int numToGet, int maxLength)
        {
            // the int is a unique ID (what we want the typeahead to return). The float is a random popularity rating.
            return Enumerable.Range(min, numToGet).Select(x => new Tuple<string, int, float>(GetRandomString(RandomGenerator.GetRandom(1, maxLength)), x, RandomGenerator.GetRandom(0.0F, 100.0F))).ToList();
        }

        private static string GetRandomString(int length)
        {
            char[] charsToUse = new char[] { 'a', 'b', 'c', 'd' };
            return new string(Enumerable.Range(0, length).Select(x => charsToUse[RandomGenerator.GetRandom(0, charsToUse.Length - 1)]).ToArray());
        }
    }
}
