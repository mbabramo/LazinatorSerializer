using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.CodeDescription
{
    public class CodeStringBuilder
    {
        StringBuilder sb = new StringBuilder();
        public int SpacesPerTab = 4;
        public int IndentLevel = 0;
        public bool IsBeginningOfLine = true;
        public static bool AddLocationIndexComments = true; // DEBUG
        public static int StopAtLocationIndex = -1;
        public static int LocationIndex = 0;

        public override string ToString()
        {
            return sb.ToString();
        }

        public void AppendLine(string s = null)
        {
            if (s != null)
                Append(s);
            Append('\r');
            Append('\n');
        }

        public void Append(string s)
        {
            if (AddLocationIndexComments && !LastLineStartsWithHash(s))
            {
                AppendHelper(GetNextLocationString());
            }
            AppendHelper(s);
        }


        private void IndentToCurrentLevel()
        {
            int spaces = SpacesPerTab * IndentLevel;
            for (int i = 0; i < spaces; i++)
                sb.Append(' ');
        }

        public string GetNextLocationString()
        {
            if (LocationIndex == StopAtLocationIndex)
            {
                Debugger.Break();
            }
            if (AddLocationIndexComments)
            {
                bool inPreprocessorLine = LastLineStartsWithHash(sb.ToString());
                if (inPreprocessorLine)
                    return $"//Location{LocationIndex++}";
                else
                    return $"/*Location{LocationIndex++}*/";
            }
            else
                return "";
        }

        static bool LastLineStartsWithHash(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            // Find the last newline character in the string
            int lastIndex = input.Length - 1;
            while (lastIndex >= 0 && input[lastIndex] != '\n')
            {
                lastIndex--;
            }

            // lastIndex now points to the '\n' of the last line or -1 if no newline is found
            int startOfLastLine = lastIndex + 1; // This will be the start of the last line or 0 if the whole input is one line

            // Find the first non-whitespace character in the last line
            for (int i = startOfLastLine; i < input.Length; i++)
            {
                if (!char.IsWhiteSpace(input[i]))
                {
                    return input[i] == '#';
                }
            }

            // If we reach here, the last line is empty or contains only whitespace
            return false;
        }


        private void AppendHelper(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                Append(c);
            }
        }

        private void Append(char c)
        {
            if (IsBeginningOfLine)
            {
                // ignore spaces and tabs at the beginning of a lie
                if (c != ' ' && c != '\t')
                {
                    // we're not at the beginning of a line
                    if (c == '{')
                    {
                        IndentToCurrentLevel();
                        IndentLevel++;
                    }
                    else if (c == '}')
                    {
                        IndentLevel--;
                        if (IndentLevel < 0)
                            throw new LazinatorCodeGenException("Internal Lazinator error. Unmatched closing brace.");
                        IndentToCurrentLevel();
                    }
                    else
                        IndentToCurrentLevel();
                    IsBeginningOfLine = false;
                }
            }

            if (!IsBeginningOfLine)
            {
                sb.Append(c);
                IsBeginningOfLine = c == '\n';
            }
        }

        
    }
}
