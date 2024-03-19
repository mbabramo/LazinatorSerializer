using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator.Utility
{
    internal class CodeFormatter
    {
        public static string IndentCode(string code)
        {
            StringBuilder indentedCode = new StringBuilder();
            int indentLevel = 0;
            string indentUnit = "    "; // 4 spaces for each indent level

            string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // Decrease indent level if the line contains only a closing brace
                if (trimmedLine == "}")
                {
                    indentLevel = Math.Max(indentLevel - 1, 0);
                }

                // Apply current indent
                indentedCode.Append(new String(' ', indentLevel * indentUnit.Length));
                indentedCode.AppendLine(line.Trim());

                // Increase indent level if the line contains only an opening brace
                if (trimmedLine == "{")
                {
                    indentLevel++;
                }
            }

            return indentedCode.ToString();
        }
    }
}
