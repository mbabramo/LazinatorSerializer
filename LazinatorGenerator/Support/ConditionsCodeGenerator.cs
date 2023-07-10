using System.Collections.Generic;
using System.Linq;

namespace LazinatorGenerator.Support
{
    public class ConditionsCodeGenerator : ConditionCodeGenerator
    {
        public List<ConditionCodeGenerator> Items;
        public bool CombineWithAnd;
        public bool CombineWithOr => !CombineWithAnd;
        string Connector => CombineWithAnd ? " && " : " || ";

        public ConditionsCodeGenerator(List<ConditionCodeGenerator> items, bool combineWithAnd) : base(null)
        {
            Items = items;
            CombineWithAnd = combineWithAnd;
        }

        public static ConditionsCodeGenerator AndCombine(ConditionCodeGenerator condition1, ConditionCodeGenerator condition2) => new ConditionsCodeGenerator(new List<ConditionCodeGenerator>() { condition1, condition2 }, true);
        public static ConditionsCodeGenerator OrCombine(ConditionCodeGenerator condition1, ConditionCodeGenerator condition2) => new ConditionsCodeGenerator(new List<ConditionCodeGenerator>() { condition1, condition2 }, false);

        public override string ToString()
        {
            List<string> itemsAsStrings = Items.Select(x =>
            {
                string itemString = x.ToString();
                if (x is ConditionsCodeGenerator y && y.CombineWithAnd != CombineWithAnd)
                    itemString = $"({itemString})"; // for example, if this is A && B && C but B is D || E, then we have A && (D || E) && C
                return itemString;
            })
                .Where(x => !(CombineWithAnd && (x == "true" || x == "(true)")))
                .Where(x => !(CombineWithOr && (x == "false" || x == "(false)")))
                .ToList();
            if (!itemsAsStrings.Any())
                return "true";
            if (CombineWithAnd && itemsAsStrings.Any(x => (x == "false" || x == "(false)")))
                return "false";
            if (CombineWithOr && itemsAsStrings.Any(x => (x == "true" || x == "(true)")))
                return "true";
            return string.Join(Connector, itemsAsStrings);
        }


    }
}
