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
        public static bool AddLocationIndexComments = false;
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
                            throw new LazinatorCodeGenException("Internal error. Unmatched closing brace.");
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

        



        public string ConditionalText(string startCommandEnclosure, string endCommandEnclosure)

            // The advantage of the following approach is that we can accumulate information and spit out the sync & async results. The disadvantage is that we might need multiple calls. 
            // First, if we're not doing async, we want things to get processed absolutely normally. So, this suggests that we want to just be doing our ordinary string building and then we have conditionals. That is, if we're in an include async mode, then we'll include some special code. Then, at the very end, we'll go back and do search/replace for the code, noting where methods begin, whether we call await, etc. For example, we could replace matching $$A( and $$A) brackets for the asynchronous code and $$N( and $$N) for the not asynchronous 

        public bool PossiblyAsyncMethodActive;
        public bool IncludeAsync;
        public CodeStringBuilder AsyncVersion;
        public CodeStringBuilder NotAsyncVersion;

        public void BeginPossiblyAsync(bool includeAsync)
        {
            PossiblyAsyncMethodActive = true;
            IncludeAsync = includeAsync;
            AsyncVersion = new CodeStringBuilder();
            NotAsyncVersion = new CodeStringBuilder();
        }

        public void EndPossiblyAsync()
        {
            PossiblyAsyncMethodActive = false;
            AppendLine(NotAsyncVersion.ToString());
            if (IncludeAsync)
                AppendLine(AsyncVersion.ToString());
            AsyncVersion = null;
            NotAsyncVersion = null;
        }
    }
}
