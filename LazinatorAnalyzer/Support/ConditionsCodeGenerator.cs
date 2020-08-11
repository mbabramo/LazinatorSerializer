using System.Collections.Generic;
using System.Linq;

namespace LazinatorAnalyzer.Support
{
    public class ConditionsCodeGenerator : ConditionCodeGenerator
    {
        public List<ConditionCodeGenerator> Items;
        public bool And;
        string Connector => And ? " && " : " || ";

        public ConditionsCodeGenerator(List<ConditionCodeGenerator> items, bool and) : base(null)
        {
            Items = items;
            And = and;
        }

        public static ConditionsCodeGenerator AndCombine(ConditionCodeGenerator condition1, ConditionCodeGenerator condition2) => new ConditionsCodeGenerator(new List<ConditionCodeGenerator>() { condition1, condition2 }, true);
        public static ConditionsCodeGenerator OrCombine(ConditionCodeGenerator condition1, ConditionCodeGenerator condition2) => new ConditionsCodeGenerator(new List<ConditionCodeGenerator>() { condition1, condition2 }, false);

        public override string ToString()
        {
            List<string> itemsAsStrings = Items.Select(x =>
            {
                string itemString = x.ToString();
                if (x is ConditionsCodeGenerator y && y.And != And)
                    itemString = $"({itemString})"; // for example, if this is A && B && C but B is D || E, then we have A && (D || E) && C
                return itemString;
            }).ToList();
            if (And && itemsAsStrings.Any(x => (x == "false" || x == "(false)")))
                return "false";
            if (!And /* i.e., OR */ && itemsAsStrings.Any(x => (x == "true" || x == "(true)")))
                return "true";
            return string.Join(Connector, itemsAsStrings);
        }


    }
}
