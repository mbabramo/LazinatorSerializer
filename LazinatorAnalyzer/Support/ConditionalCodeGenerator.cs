using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAnalyzer.Support
{
    public class ConditionalCodeGenerator
    {
        public ConditionCodeGenerator Antecedent;
        public string Consequent;
        public string ElseConsequent;

        public ConditionalCodeGenerator(ConditionCodeGenerator antecedent, string consequent, string elseConsequent = null)
        {
            Antecedent = antecedent;
            Consequent = consequent;
            ElseConsequent = elseConsequent;
        }

        public static string ConsequentPossibleOnlyIf(bool onlyIfCondition, ConditionCodeGenerator antecedent, string consequent, string elseConsequent = null) => onlyIfCondition ? new ConditionalCodeGenerator(antecedent, consequent, elseConsequent).ToString() : elseConsequent;
        public static string ElseConsequentPossibleOnlyIf(bool onlyIfCondition, ConditionCodeGenerator antecedent, string consequent, string elseConsequent = null) => onlyIfCondition ? new ConditionalCodeGenerator(antecedent, consequent, elseConsequent).ToString() : consequent;

        public override string ToString()
        {
            string antecedentString = (Antecedent?.ToString() ?? "").Trim();
            if (antecedentString.StartsWith("if (") && antecedentString.EndsWith(")"))
            {
                antecedentString = antecedentString.Substring(4, antecedentString.Length - 5);
            }
            if (antecedentString == null || antecedentString == "" || antecedentString == "true" || antecedentString == "(true)")
                return Consequent;
            if (antecedentString == "false" || antecedentString == "(false)")
                return ElseConsequent;
            string mainString = $@"if ({antecedentString})
                    {{
                        {Consequent}
                    }}";
            if (ElseConsequent != null && ElseConsequent != "")
                mainString += $@"
                    else
                    {{
                        {ElseConsequent}
                    }}";
            return mainString;

        }

    }
}
