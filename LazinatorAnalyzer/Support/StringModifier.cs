using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LazinatorAnalyzer.Support
{
    public static class StringModifier
    {

        public class Tree<T> : List<Tree<T>>
        {
            public T Data { get; set; }

            public Tree(T data)
            {
                this.Data = data;
            }

            public IEnumerable<T> WalkContents()
            {
                foreach (Tree<T> child in this)
                {
                    foreach (var descendant in child.WalkContents())
                        yield return descendant;
                }
                yield return this.Data;
            }

            public IEnumerable<Tree<T>> WalkTreeNodes()
            {
                foreach (Tree<T> child in this)
                {
                    foreach (var descendant in child.WalkTreeNodes())
                        yield return descendant;
                }
                yield return this;
            }

            public string StringProducer(Func<Tree<T>, string> leafContentsFunc, Func<Tree<T>, string, string> transformStringFunc)
            {
                if (this.Any())
                {
                    IEnumerable<string> childStrings = this.Select(x => x.StringProducer(leafContentsFunc, transformStringFunc));
                    StringBuilder sb = new StringBuilder();
                    foreach (var childString in childStrings)
                        sb.AppendLine(childString);
                    return transformStringFunc(this, sb.ToString());
                }
                else
                {
                    string contentsString = leafContentsFunc(this);
                    return transformStringFunc(this, contentsString);
                }
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

        public abstract class TextBlockBase
        {
            public TextBlockBase()
            {

            }
            public abstract (int startRange, int endRange) OverallRange { get; }
            public abstract (int startRange, int endRange)? ContentRange { get; }
            public abstract (int startRange, int endRange)? CommandRange { get; }
            public (string commandName, string commandContent) GetCommand(string overallString)
            {
                if (CommandRange == null)
                    return ("", null);
                string overallCommand = overallString.Substring(CommandRange.Value.startRange, CommandRange.Value.endRange - CommandRange.Value.startRange + 1);
                int index = overallCommand.IndexOf('=');
                if (index > 0)
                {
                    // e.g., a=b corresponds to indices 012 so we get commandName at 0 with length 1, and commandContent at 2 with length 2 - 1
                    string commandName = overallCommand.Substring(0, index);
                    string commandContent = overallCommand.Substring(index + 1, overallCommand.Length - index);
                    return (commandName, commandContent);
                }
                return (overallCommand, "");
            }

            public string GetContent(string overallString) => ContentRange == null ? null : overallString.Substring(ContentRange.Value.startRange, ContentRange.Value.endRange - ContentRange.Value.startRange + 1);
        }

        public class TextBlock : TextBlockBase
        {
            public TextBlock(int startRange, int endRange)
            {
                _ContentRange = (startRange, endRange);
            }

            (int startRange, int endRange) _ContentRange;
            public override (int startRange, int endRange) OverallRange => _ContentRange;
            public override (int startRange, int endRange)? CommandRange => null;
            public override (int startRange, int endRange)? ContentRange => _ContentRange;
        }

        private class CommandWithTextBlock : TextBlockBase
        {
            public CommandWithTextBlock(CommandLocation startCommandLocation, CommandLocation endCommandLocation)
            {
                StartCommandLocation = startCommandLocation;
                EndCommandLocation = endCommandLocation;
            }

            public CommandLocation StartCommandLocation;
            public CommandLocation EndCommandLocation;
            public override (int startRange, int endRange) OverallRange => (StartCommandLocation.OverallStart, EndCommandLocation.OverallEnd);

            public override (int startRange, int endRange)? CommandRange => (StartCommandLocation.InnerStart, StartCommandLocation.InnerEnd);
            public override (int startRange, int endRange)? ContentRange => EndCommandLocation.OverallStart == StartCommandLocation.OverallEnd + 1 ? null : (StartCommandLocation.OverallEnd + 1, EndCommandLocation.OverallStart - 1);
        }

        private static List<int> AllIndexesOf(this string str, string value)
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

        private static List<(int, int)> AllIndexRangesOf(this string str, string value) => AllIndexesOf(str, value).Select(x => (x, x + value.Length)).ToList();

        private static List<CommandLocation> GetCommandLocations(this string str, string openDelimeter, string closeDelimeter)
        {
            var openDelimeterLocations = AllIndexRangesOf(str, openDelimeter);
            var closeDelimeterLocations = AllIndexRangesOf(str, closeDelimeter);
            var matchingCloseDelimeterLocations = openDelimeterLocations.Select(x => closeDelimeterLocations.SkipWhile(y => y.Item1 < x.Item1).First());
            return openDelimeterLocations
                .Zip(matchingCloseDelimeterLocations, (s, e) => (s.Item1, s.Item2 + 1, e.Item1 - 1, e.Item2))
                .Where(x => x.Item3 >= x.Item2) // omit commands with no internal content
                .Select(x => new CommandLocation(x.Item1, x.Item2, x.Item3, x.Item4))
                .ToList();
        }

        private static Tree<TextBlockBase> GetCommandTree(this string str, string beginCommandOpenDelimeter, string endCommandOpenDelimeter, string closeDelimeter)
        {
            // Assemble a list of begin and end commands
            var beginCommands = GetCommandLocations(str, beginCommandOpenDelimeter, closeDelimeter);
            var endCommands = GetCommandLocations(str, endCommandOpenDelimeter, closeDelimeter);
            if (beginCommands.Count() != endCommands.Count)
                throw new ArgumentException("Mismatched commands");
            // Order the commands based on their location in the document. 
            List<(CommandLocation location, bool isBegin)> allCommandLocations = beginCommands.Select(x => (x, true)).ToList();
            allCommandLocations.AddRange(endCommands.Select(x => (x, false)));
            allCommandLocations = allCommandLocations.OrderBy(x => x.location.OverallStart).ToList();
            // We now need to match begin and end commands into CommandWithTextBlock objects. These then need to be nested,
            // and for that we use a tree structure.
            // Create a tree, where each tree is a list of its children and has data corresponding to the text block.
            // The base node of the tree will be the overall text block (without any corresponding command).
            Stack<Tree<TextBlockBase>> textBlockTreeStack = new Stack<Tree<TextBlockBase>>();
            Tree<TextBlockBase> treeBase = new Tree<TextBlockBase>(new TextBlock(0, str.Length - 1));
            textBlockTreeStack.Push(treeBase);
            Stack<CommandLocation> commandLocationStack = new Stack<CommandLocation>();
            foreach (var commandLocation in allCommandLocations)
            {
                if (commandLocation.isBegin)
                {
                    commandLocationStack.Push(commandLocation.location);
                    textBlockTreeStack.Push(new Tree<TextBlockBase>(default));
                }
                else
                {
                    var endCommandLocation = commandLocation.location;
                    var startCommandLocation = commandLocationStack.Pop();
                    var commandWithTextBlock = new CommandWithTextBlock(startCommandLocation, endCommandLocation);
                    var textBlockTreeNode = textBlockTreeStack.Pop();
                    FillInMissingRanges(textBlockTreeNode);
                    textBlockTreeStack.Peek().Add(textBlockTreeNode);
                }
            }
            if (treeBase != textBlockTreeStack.Peek())
                throw new Exception("Internal error processing command ranges.");
            FillInMissingRanges(treeBase);
            return treeBase;

            void FillInMissingRanges(Tree<TextBlockBase> treeNode)
            {
                if (treeNode.Any() && treeNode.Data.ContentRange != null)
                {
                    int numChildren = treeNode.Count();
                    int expectedNext = 0;
                    for (int childIndex = 0; childIndex < numChildren; childIndex++)
                    {
                        var child = treeNode[childIndex];
                        int childStart = child.Data.OverallRange.startRange;
                        if (expectedNext != childStart)
                            treeNode.Insert(childIndex, new Tree<TextBlockBase>(new TextBlock(expectedNext, childStart - 1)));
                        else
                            expectedNext = child.Data.OverallRange.endRange + 1;
                    }
                    int currentEnd = treeNode.Last().Data.OverallRange.endRange;
                    int expectedEnd = treeNode.Data.OverallRange.endRange;
                    if (currentEnd < expectedEnd)
                        treeNode.Add(new Tree<TextBlockBase>(new TextBlock(currentEnd + 1, expectedEnd)));
                }
            }

        }

        public class TransformCommand
        {
            public virtual string TransformRange(string stringToTransform, string commandContent, Tree<TextBlockBase> node)
            {
                return Transform(stringToTransform);
            }

            public virtual string Transform(string content) => content;
        }

        public static string ModifyString(this string str, string beginCommandOpenDelimeter, string endCommandOpenDelimeter, string closeDelimeter, Dictionary<string, TransformCommand> transformers)
        {
            Tree<TextBlockBase> tree = GetCommandTree(str, beginCommandOpenDelimeter, endCommandOpenDelimeter, closeDelimeter);
            string result = tree.StringProducer(
                treeNode => treeNode.Data.GetContent(str),
                (treeNode, originalString) =>
                {
                    TextBlockBase block = treeNode.Data;
                    var command = block.GetCommand(str);
                    if (command.commandName == "")
                        return originalString;
                    else
                    {
                        if (transformers.ContainsKey(command.commandName))
                        {
                            string transformedString = transformers[command.commandName].TransformRange(str, command.commandContent, treeNode);
                            return transformedString;
                        }
                        else
                            throw new Exception("Transformer {command.commandName} not defined");
                    }
                });
            return result;
        }
    }
}
