using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LazinatorAnalyzer.Support
{
    public class StringTemplates
    {
        public string BeginCommandOpenDelimeter;
        public string EndCommandOpenDelimeter;
        public string CloseDelimeter;

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

            public string StringProducer(Func<Tree<T>, string> leafContentsFunc, Func<Tree<T>, string, int> orderFunc, Func<Tree<T>, string, string> transformStringFunc)
            {
                Debug; // we need to add an ordering here.
                if (this.Any())
                {
                    List<int> order = this.Select(x => orderFunc(this, ))
                    IEnumerable<string> childStrings = this.Select(x => x.StringProducer(leafContentsFunc, orderFunc, transformStringFunc));
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
            public virtual int Order => 0;
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

            Debug; // allow specification of order. move command range inward if the command begins with an order declaration.

            public CommandLocation StartCommandLocation;
            public CommandLocation EndCommandLocation;
            public override (int startRange, int endRange) OverallRange => (StartCommandLocation.OverallStart, EndCommandLocation.OverallEnd);

            public override (int startRange, int endRange)? CommandRange => (StartCommandLocation.InnerStart, StartCommandLocation.InnerEnd);
            public override (int startRange, int endRange)? ContentRange => EndCommandLocation.OverallStart == StartCommandLocation.OverallEnd + 1 ? null : (StartCommandLocation.OverallEnd + 1, EndCommandLocation.OverallStart - 1);
        }

        private static List<int> AllIndexesOf(string str, string value)
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

        private static List<(int, int)> AllIndexRangesOf(string str, string value) => AllIndexesOf(str, value).Select(x => (x, x + value.Length)).ToList();

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

        private static Tree<TextBlockBase> GetCommandTree(string str, string beginCommandOpenDelimeter, string endCommandOpenDelimeter, string closeDelimeter)
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

        public class CommandBase
        {

            public virtual int Order => 0;

            public virtual string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables, Func<string, Dictionary<string, string>, string> modifyFunc)
            {
                return stringToTransform;
            }
        }

        public class OrderCommand : CommandBase
        {
            private int _Order;
            public override int Order => _Order;

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables, Func<string, Dictionary<string, string>, string> modifyFunc)
            {
                _Order = Int32.Parse(commandContent);
                return stringToTransform; // If there are commands using the variable, the result will be processed again.
            }
        }

        public class SetVariableCommand : CommandBase
        {
            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables, Func<string, Dictionary<string, string>, string> modifyFunc)
            {
                var split = commandContent.Split(',').ToList();
                string variableName = split[0];
                string value = split[1];
                variables[variableName] = value;
                return stringToTransform; // If there are commands using the variable, the result will be processed again.
            }
        }

        public class IfVariableCommand : CommandBase
        {
            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables, Func<string, Dictionary<string, string>, string> modifyFunc)
            {
                var split = commandContent.Split(',').ToList();
                string variableName = split[0];
                string value = split[1];
                if (variables.ContainsKey(variableName) && variables[variableName] == value)
                    return stringToTransform; 
                return "";
            }
        }

        public class ForCommand : CommandBase
        {
            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables, Func<string, Dictionary<string, string>, string> modifyFunc)
            {
                // Note that we require that the content be like 2,variableName or 3,4.
                // We don't allow the content to consist of other commands that can be reduced to that.
                var split = commandContent.Split(',').ToList();
                string variableName = split[0];
                int firstValueInclusive, secondValueExclusive;
                if (!int.TryParse(split[1], out firstValueInclusive))
                    firstValueInclusive = Int32.Parse(variables[split[1]]);
                if (!int.TryParse(split[2], out secondValueExclusive))
                    secondValueExclusive = Int32.Parse(variables[split[2]]);
                StringBuilder sb = new StringBuilder();
                for (int i = firstValueInclusive; i < secondValueExclusive; i++)
                {
                    variables[variableName] = i.ToString();
                    sb.AppendLine(modifyFunc(stringToTransform, variables));
                }
                return sb.ToString();
            }
        }

        public static Dictionary<string, CommandBase> Transformers = new Dictionary<string, CommandBase>()
        {
            { "", new CommandBase() },
            { "set", new SetVariableCommand() },
            { "if", new IfVariableCommand() },
            { "for", new ForCommand() },
            { "order", new OrderCommand() }
        };

        public string ModifyString( string str, Dictionary<string, string> variables)
        {
            Tree<TextBlockBase> tree = GetCommandTree(str, BeginCommandOpenDelimeter, EndCommandOpenDelimeter, CloseDelimeter);
            string result = tree.StringProducer(
                treeNode =>
                {
                    string content = treeNode.Data.GetContent(str);
                    while (content.IndexOf(BeginCommandOpenDelimeter) >= 0)
                        content = ModifyString(content, variables);
                    return content;
                },
                (treeNode, originalString) =>
                {
                    TextBlockBase block = treeNode.Data;
                    var command = block.GetCommand(str);
                    return Transformers[command.commandName].Order;
                },
                (treeNode, originalString) =>
                {
                    TextBlockBase block = treeNode.Data;
                    var command = block.GetCommand(str);
                    string commandContent = command.commandContent;
                    while (commandContent.IndexOf(BeginCommandOpenDelimeter) >= 0)
                        commandContent = ModifyString(commandContent, variables);
                    if (command.commandName == "")
                        return originalString;
                    else
                    {
                        if (Transformers.ContainsKey(command.commandName))
                        {
                            string transformedString = Transformers[command.commandName].TransformRange(str, commandContent, variables, ModifyString);
                            return transformedString;
                        }
                        else
                            throw new Exception("Transformer {command.commandName} not defined");
                    }
                };
            return result;
        }
    }
}
