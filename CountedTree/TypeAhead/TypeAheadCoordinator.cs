using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace CountedTree.TypeAhead
{
    /// <summary>
    /// Allows a client to obtain type-ahead results. This class does the work of adding results to and removing them from all applicable nodes, and loading more results when necessary. For example, if the client adds a result for the search "HORSE", this class will try to add the result to "H", "HO", "HOR", "HORS", and "HORSE", if its popularity is high enough to be maintained there. If "HORSE" were removed, then this would be removed from all those nodes, and if necessary, the nodes based on shorter strings would look to their children for more results (so "H" might look to "HA", "HE", "HI", "HO", etc., with the next characters based on what has been added in the past).
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class TypeAheadCoordinator<R> where R : ILazinator,
          IComparable,
          IComparable<R>,
          IEquatable<R>
    {
        ITypeAheadStorage<R> Executor;

        public TypeAheadCoordinator(ITypeAheadStorage<R> executor)
        {
            Executor = executor;
        }

        public async Task AddResult(Guid context, string searchString, TypeAheadItem<R> typeAheadItem)
        {
            searchString = Reduce(searchString);
            if (searchString != null && searchString.Length > 0)
            {
                char[] charArray = searchString.ToCharArray();
                for (int i = charArray.Length - 1; i >= 0; i--)
                {
                    string partialString = new string(charArray.Take(i + 1).ToArray());
                    char? nextChar = (i == charArray.Length - 1) ? null : (char?)charArray[i + 1];
                    bool added = await AddPartialResult(context, partialString, nextChar, typeAheadItem);
                    if (!added)
                        break; // if we didn't add here, we certainly won't need to add higher up the chain
                }
            }
        }

        public async Task<List<R>> GetSearchResults(Guid context, string searchString, int numToTake)
        {
            searchString = Reduce(searchString);
            Guid nodeGuid = GetNodeGuid(context, searchString);
            return await Executor.GetSearchResults(nodeGuid, numToTake);
        }

        public async Task<List<TypeAheadItem<R>>> GetTypeAheadItems(Guid context, string searchString, int numToTake, bool prioritizeExactMatches = false)
        {
            searchString = Reduce(searchString);
            Guid nodeGuid = GetNodeGuid(context, searchString);
            return await Executor.GetTypeAheadItems(nodeGuid, numToTake, prioritizeExactMatches);
        }

        public async Task<TypeAheadItemsForParent<R>> GetItemsForParent(Guid context, string searchString, TypeAheadItem<R> lowestItemOnParent, int maxNumResults)
        {
            searchString = Reduce(searchString);
            Guid nodeGuid = GetNodeGuid(context, searchString);
            return await Executor.GetItemsForParent(nodeGuid, searchString, lowestItemOnParent, maxNumResults);
        }

        public Task RemoveResult(Guid context, string searchString, R result)
        {
            searchString = Reduce(searchString);
            if (searchString != null && searchString.Length > 0)
            {
                char[] charArray = searchString.ToCharArray();
                Task[] results = new Task[charArray.Length];
                // We must go backwards, from longest to shortest. Otherwise, when we delete from a short version, it may then add right away from the next longer version. Also, if we have to refill buffers because we've gone below the minimum, we want to make sure to do this on the longer items first, so that when the shorter items request their children, they get a comprehensive list.
                for (int i = charArray.Length - 1; i >= 0; i--)
                {
                    string partialString = new string(charArray.Take(i + 1).ToArray());
                    results[i] = RemovePartialResult(context, partialString, result);
                }
                return Task.WhenAll(results);
            }
            return Task.CompletedTask;
        }

        public async Task ChangePopularity(Guid context, string searchString, TypeAheadItem<R> typeAheadItem)
        {
            // We must manually remove and add the results, because we want to make sure that we refill buffers if necessary before adding the result.
            await RemoveResult(context, searchString, typeAheadItem.SearchResult);
            await AddResult(context, searchString, typeAheadItem);
        }

        private string Reduce(string original)
        {
            return LimitToLettersAndDigits(RemoveDiacritics(original)).ToUpper();
        }

        private static string LimitToLettersAndDigits
            (string input)
        {
            return new string(input.ToCharArray()
                .Where(c => Char.IsLetterOrDigit(c))
                .ToArray());

        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private Guid GetNodeGuid(Guid context, string partialResultString)
        {
            return MD5HashGenerator.GetDeterministicGuid(new Tuple<Guid, string>(context, partialResultString));
        }

        private Task<bool> AddPartialResult(Guid context, string partialResultString, char? nextChar, TypeAheadItem<R> typeAheadItem)
        {
            Guid nodeGuid = GetNodeGuid(context, partialResultString);
            return Executor.AddTypeAheadItem(partialResultString, nodeGuid, nextChar, typeAheadItem);
        }

        private async Task RemovePartialResult(Guid context, string partialResultString, R result)
        {
            Guid nodeGuid = GetNodeGuid(context, partialResultString);
            MoreResultsRequest<R> request = await Executor.RemoveResult(nodeGuid, result);
            if (request != null)
                await FulfillMoreResultsRequest(context, partialResultString, request);
        }

        private async Task FulfillMoreResultsRequest(Guid context, string partialResultString, MoreResultsRequest<R> request)
        {
            List<TypeAheadItem<R>> aggregatedResults = new List<TypeAheadItem<R>>();
            // We will look at child nodes for their items, but also may need to look to descendant nodes if child(ren) nodes have rejected items that would have beaten the best results.
            List<string> childNodeStrings = request.NextChars.Select(c => partialResultString + c).ToList();
            while (childNodeStrings.Any())
            {
                Task<TypeAheadItemsForParent<R>>[] tasks = new Task<TypeAheadItemsForParent<R>>[childNodeStrings.Count()];
                int i = 0;
                foreach (string childNodeString in childNodeStrings)
                {
                    tasks[i] = GetItemsForParent(context, childNodeString, request.LowestExistingItem, request.MaxNumResults);
                    i++;
                }
                await Task.WhenAll(tasks);
                List<TypeAheadItemsForParent<R>> resultsForEachChild = tasks.Select(x => x.Result).ToList();
                var setsOfCandidates = resultsForEachChild.Select(x => x.CandidatesForParent.Where(y => !aggregatedResults.Any(z => z.Equals(y))));
                var merged = setsOfCandidates.MergeOrderedEnumerables().Take(request.MaxNumResults);
                aggregatedResults.AddRange(merged);
                aggregatedResults = aggregatedResults.OrderBy(x => x).Take(request.MaxNumResults).ToList();
                childNodeStrings = new List<string>();
                if (aggregatedResults.Any())
                {
                    var lowest = aggregatedResults.Last();
                    var mustLookToGrandchildren = resultsForEachChild.Where(x => x.MostPopularRejected != null && x.MostPopularRejected.CompareTo(lowest) < 0);
                    foreach (var g in mustLookToGrandchildren)
                        foreach (var c in g.NextChars)
                            childNodeStrings.Add(g.SearchString + c);
                }
            }
            Guid nodeGuid = GetNodeGuid(context, partialResultString);
            await Executor.AddTypeAheadItemsFromChildren(nodeGuid, aggregatedResults);
        }
    }
}
