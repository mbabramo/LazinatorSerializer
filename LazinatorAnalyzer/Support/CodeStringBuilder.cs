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

        /* We need some method of parsing the string around start command and end command delimeters into a tree. */
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        private class Tree<T> : List<Tree<T>>
        {
            public T Data { get; private set; }

            public Tree(T data)
            {
                this.Data = data;
            }

        }

        private struct CommandLocation
        {
            public CommandLocation(int overallStart, int overallEnd, int innerStart, int innerEnd)
            {
                OverallStart = overallStart;
                OverallEnd = overallEnd;
                InnerStart = innerStart;
                InnerEnd = innerEnd;
            }
            public int OverallStart;
            public int OverallEnd;
            public int InnerStart;
            public int InnerEnd;
        }

        private struct CommandBlock
        {
            public CommandLocation StartCommandLocation;
            public CommandLocation EndCommandLocation;
            public (int startRange, int endRange) OverallRange => (StartCommandLocation.OverallStart, EndCommandLocation.OverallEnd);
            public (int startRange, int endRange)? InnerRange => EndCommandLocation.OverallStart == StartCommandLocation.OverallEnd + 1 ? null : (StartCommandLocation.OverallEnd + 1, EndCommandLocation.OverallStart - 1);
        }

        private static List<(int, int)> AllIndexRangesOf(this string str, string value) => AllIndexesOf(str, value).Select(x => (x, x + value.Length)).ToList();

        private static List<CommandLocation> GetCommandLocations(this string str, string openCommandText, string closeCommandText)
        {
            var startCommandLocations = AllIndexRangesOf(str, openCommandText);
            var endCommandLocations = AllIndexRangesOf(str, closeCommandText);
            if (startCommandLocations.Count() != endCommandLocations.Count())
                throw new ArgumentException("Unbalanced start and end commands.");
            return startCommandLocations
                .Zip(endCommandLocations, (s, e) => (s.Item1, s.Item2 + 1, e.Item1 - 1, e.Item2))
                .Where(x => x.Item3 >= x.Item2) // omit commands with no content
                .Select(x => new CommandLocation(x.Item1, x.Item2, x.Item3, x.Item4))
                .ToList();
        }

        private static Tree<CommandBlock> GetCommandRanges(this string str, string beginCommandOpenCommandText, string beginCommandCloseCommandText, string endCommandOpenCommandText, string endCommandCloseCommandText)
        {
            Debug; // change this so that we have a single close command (always the next after the end text)
            var beginCommands = GetCommandLocations(str, beginCommandOpenCommandText, beginCommandCloseCommandText);
            var endCommands = GetCommandLocations(str, endCommandOpenCommandText, endCommandCloseCommandText);
            if (beginCommands.Count() != endCommands.Count)
                throw new ArgumentException("Mismatched commands");
            List<(CommandLocation location, bool isBegin)> allCommandLocations = beginCommands.Select(x => (x, true)).ToList();
            allCommandLocations.AddRange(endCommands.Select(x => (x, false)));
            allCommandLocations = allCommandLocations.OrderBy(x => x.location.OverallStart).ToList();
            Stack<Tree<CommandBlock>> commandRangeList = new Stack<Tree<CommandBlock>>();
            Stack<CommandLocation> pendingBeginCommands = new Stack<CommandLocation>();
            foreach (var commandLocation in allCommandLocations)
            {
                if (commandLocation.isBegin)
                    pendingBeginCommands.Push(commandLocation.location);
                else
                {

                }
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
