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
        public static int StopAtLocationIndex = -1; // 9731; // 13055; // DEBUG -1; 
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
            if (AddLocationIndexComments)
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

        public static string GetNextLocationString()
        {
            if (LocationIndex == StopAtLocationIndex)
            {
                Debugger.Break();
            }
            return AddLocationIndexComments ? $"/*Location{LocationIndex++}*/" : "";
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
