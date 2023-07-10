using System;
using System.Text;

namespace LazinatorGenerator.Support
{
    public class ConditionCodeGenerator
    {
        string Antecedent;

        public ConditionCodeGenerator(string antecedent)
        {
            Antecedent = antecedent;
        }

        public static implicit operator string(ConditionCodeGenerator c) => c.ToString();
        public static implicit operator ConditionCodeGenerator(string a) => new ConditionCodeGenerator(a);

        public override string ToString()
        {
            return Antecedent;
        }
    }
}
