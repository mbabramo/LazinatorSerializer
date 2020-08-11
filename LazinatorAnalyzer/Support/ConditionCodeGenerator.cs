using System;
using System.Text;

namespace LazinatorAnalyzer.Support
{
    public class ConditionCodeGenerator
    {
        string Antecedent;

        public ConditionCodeGenerator(string antecedent)
        {
            Antecedent = antecedent;
        }

        public static implicit operator string(ConditionCodeGenerator c) => c.ToString();
        public static explicit operator ConditionCodeGenerator(string a) => new ConditionCodeGenerator(a);

        public override string ToString()
        {
            return Antecedent;
        }
    }
}
