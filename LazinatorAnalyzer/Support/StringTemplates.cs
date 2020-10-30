using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LazinatorAnalyzer.Support
{
    /// <summary>
    /// A very simple string templating engine that can handle if and for commands, and variable values. A command block consists of a begin and end command, each with an 
    /// open and close delimeter. Between the two commands is the text to be transformed. A parent command is always processed after each of the children
    /// is processed. 
    /// </summary>
    public class StringTemplates
    {
        public string BeginCommandOpenDelimeter = "/*<$$ ";
        public string EndCommandOpenDelimeter = "/*<$$/";
        public string CloseDelimeter = " $$>*/";

        public string CreateBeginCommand(string commandName, string commandContent) => $"{BeginCommandOpenDelimeter}{commandName}={commandContent}{CloseDelimeter}";

        public string CreateEndCommand(string commandName) => $"{EndCommandOpenDelimeter}{commandName}{CloseDelimeter}";

        public string CreateCommandBlock(string commandName, string commandContent, string textContent)
        {
            return $"{CreateBeginCommand(commandName, commandContent)}{textContent}{CreateEndCommand(commandName)}";
        }

        public string EncodeCommandBlock(string text, bool unencode = false)
        {
            string beginReplacement = "$$$$B";
            string endReplacement = "$$$$E";
            string closeReplacement = "$$$$>";
            if (unencode)
                return text
                    .Replace(beginReplacement, BeginCommandOpenDelimeter)
                    .Replace(endReplacement, EndCommandOpenDelimeter)
                    .Replace(closeReplacement, CloseDelimeter);
            return text
                .Replace(BeginCommandOpenDelimeter, beginReplacement)
                .Replace(EndCommandOpenDelimeter, endReplacement)
                .Replace(CloseDelimeter, closeReplacement);
        }

        public string CreateReprocessBlock(string textContent, int numCyclesLeft) => CreateCommandBlock("reprocess", numCyclesLeft.ToString(), numCyclesLeft >= 1 ? EncodeCommandBlock(textContent) : textContent);
        public string CreateReprocessBlock_BeginOnly(int numCyclesLeft) => CreateBeginCommand("reprocess", numCyclesLeft.ToString());
        public string CreateIfBlock(string variableName, string variableValue, string textContent) => CreateCommandBlock("if", $"{variableName},{variableValue}", textContent);
        public string CreateForBlock(string variableName, int startValue, int endValueExclusive, string textContent) => CreateCommandBlock("for", $"{variableName},{startValue},{endValueExclusive}", textContent);

        public string CreateForBlock_Begin(string variableName, int startValue, int endValueExclusive) => CreateBeginCommand("for", $"{variableName},{startValue},{endValueExclusive}");
        public string CreateVariableBlock(string variableName) => CreateCommandBlock("var", variableName, null);
        public string CreateSetVariableBlock(string variableName, string value) => CreateCommandBlock("set", value == null ? variableName : $"{variableName},{value}", null);
        public string CreateContainsBlock(string variableName, string textToFind, string textContent) => CreateCommandBlock("contains", $"{variableName},{textToFind}", textContent);

        private class Tree<T> : List<Tree<T>>
        {
            public T Data { get; set; }

            public Tree(T data)
            {
                this.Data = data;
            }

            public IEnumerable<T> WalkContents()
            {
                yield return this.Data;
                foreach (Tree<T> child in this)
                {
                    foreach (var descendant in child.WalkContents())
                        yield return descendant;
                }
            }

            public IEnumerable<(T item, int level)> WalkContents(int level)
            {
                yield return (this.Data, level);
                foreach (Tree<T> child in this)
                {
                    foreach (var descendant in child.WalkContents(level + 1))
                        yield return descendant;
                }
            }

            public void BuildTreeString(StringBuilder sb, Func<T, string> itemStringFunc)
            {
                foreach ((T item, int level) in WalkContents(0))
                {
                    for (int i = 0; i < 4 * level; i++)
                        sb.Append(" ");
                    sb.AppendLine(itemStringFunc(item));
                }
            }

            public string GetTreeString(Func<T, string> itemStringFunc)
            {
                StringBuilder sb = new StringBuilder();
                BuildTreeString(sb, itemStringFunc);
                return sb.ToString();
            }

            public IEnumerable<Tree<T>> WalkTreeNodes()
            {
                yield return this;
                foreach (Tree<T> child in this)
                {
                    foreach (var descendant in child.WalkTreeNodes())
                        yield return descendant;
                }
            }

            public string StringProducer(Func<Tree<T>, string> leafContentsFunc, Func<Tree<T>, string, string> transformStringFunc)
            {
                if (this.Any())
                {
                    IEnumerable<string> childStrings = this.Select(x => x.StringProducer(leafContentsFunc, transformStringFunc));
                    StringBuilder sb = new StringBuilder();
                    foreach (var childString in childStrings)
                        sb.Append(childString);
                    return transformStringFunc(this, sb.ToString());
                }
                else
                {
                    string contentsString = leafContentsFunc(this);
                    string transformed = transformStringFunc(this, contentsString);
                    return transformed;
                }
            }

        }

        /// <summary>
        /// The location of either a begin command or an end command. The inner location means everything after the delimeter.
        /// </summary>
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

            public string InnerString(string str) => str.Substring(InnerStart, InnerEnd - InnerStart + 1);
        }

        private abstract class TextBlockBase
        {
            public TextBlockBase()
            {

            }
            public virtual int Order => 0;
            public abstract (int startRange, int endRange) OverallRange { get; }
            public abstract (int startRange, int endRange)? InnerContentRange { get; }
            public abstract (int startRange, int endRange)? CommandRange { get; }
            public (string commandName, string commandContent) GetCommand(string overallString)
            {
                if (CommandRange == null)
                    return ("", null);
                string overallCommand = overallString.Substring(CommandRange.Value.startRange, CommandRange.Value.endRange - CommandRange.Value.startRange + 1);
                int index = overallCommand.IndexOf('=');
                if (index == 0)
                    return ("", "");
                if (index > 0)
                {
                    // e.g., a=b corresponds to indices 012 so we get commandName at 0 with length 1, and commandContent at 2 with length 2 - 1
                    string commandName = overallCommand.Substring(0, index);
                    string commandContent = overallCommand.Substring(index + 1, overallCommand.Length - index - 1);
                    return (commandName, commandContent);
                }
                return (overallCommand, "");
            }

            public string GetInnerContent(string overallString) => InnerContentRange == null ? null : overallString.Substring(InnerContentRange.Value.startRange, InnerContentRange.Value.endRange - InnerContentRange.Value.startRange + 1);

            public virtual string ToString(string overallString)
            {
                return $@"({OverallRange.startRange}, {OverallRange.endRange}): Command {(CommandRange == null ? "" : overallString.Substring(CommandRange.Value.startRange, CommandRange.Value.endRange - CommandRange.Value.startRange + 1))} Content {(InnerContentRange == null ? "" : overallString.Substring(InnerContentRange.Value.startRange, InnerContentRange.Value.endRange - InnerContentRange.Value.startRange + 1))}"; // Overall {overallString.Substring(OverallRange.startRange, OverallRange.endRange - OverallRange.startRange + 1)}";
            }
        }

        private class TextBlock : TextBlockBase
        {
            public TextBlock(int startRange, int endRange)
            {
                _ContentRange = (startRange, endRange);
            }

            (int startRange, int endRange) _ContentRange;
            public override (int startRange, int endRange) OverallRange => _ContentRange;
            public override (int startRange, int endRange)? CommandRange => null;
            public override (int startRange, int endRange)? InnerContentRange => _ContentRange;
        }

        private class CommandWithTextBlock : TextBlockBase
        {
            public CommandWithTextBlock(CommandLocation beginCommandLocation, CommandLocation endCommandLocation, string overallString = null)
            {
                BeginCommandLocation = beginCommandLocation;
                EndCommandLocation = endCommandLocation;
                if (overallString != null)
                {
                    string commandNameFromEndCommand = endCommandLocation.InnerString(overallString);
                    string commandNameFromBeginCommand = beginCommandLocation.InnerString(overallString);
                    if (!commandNameFromBeginCommand.StartsWith(commandNameFromEndCommand))
                        throw new Exception($"Commands {commandNameFromBeginCommand} and {commandNameFromEndCommand} are mismatched.");
                }
            }

            public CommandLocation BeginCommandLocation;
            public CommandLocation EndCommandLocation;
            public override (int startRange, int endRange) OverallRange => (BeginCommandLocation.OverallStart, EndCommandLocation.OverallEnd);

            public override (int startRange, int endRange)? CommandRange => (BeginCommandLocation.InnerStart, BeginCommandLocation.InnerEnd);
            public override (int startRange, int endRange)? InnerContentRange => EndCommandLocation.OverallStart == BeginCommandLocation.OverallEnd + 1 ? null : (BeginCommandLocation.OverallEnd + 1, EndCommandLocation.OverallStart - 1);
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

        private static List<(int, int)> AllIndexRangesOf(string str, string value) => AllIndexesOf(str, value).Select(x => (x, x + value.Length - 1)).ToList();

        private static List<CommandLocation> GetCommandLocations(string str, string openDelimeter, string closeDelimeter)
        {
            var openDelimeterLocations = AllIndexRangesOf(str, openDelimeter);
            var closeDelimeterLocations = AllIndexRangesOf(str, closeDelimeter);
            var matchingCloseDelimeterLocations = openDelimeterLocations.Select(x => closeDelimeterLocations.SkipWhile(y => y.Item1 < x.Item1).First());
            return openDelimeterLocations
                .Zip(matchingCloseDelimeterLocations, (s, e) => (s.Item1, s.Item2, e.Item1, e.Item2))
                .Where(x => x.Item3 > x.Item2) 
                .Select(x => new CommandLocation(x.Item1, x.Item4, x.Item2 + 1, x.Item3 - 1))
                .ToList();
        }

        private static Tree<TextBlockBase> GetCommandTree(string templateString, string beginCommandOpenDelimeter, string endCommandOpenDelimeter, string closeDelimeter)
        {
            // Assemble a list of begin and end commands
            var beginCommands = GetCommandLocations(templateString, beginCommandOpenDelimeter, closeDelimeter);
            var endCommands = GetCommandLocations(templateString, endCommandOpenDelimeter, closeDelimeter);
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
            Tree<TextBlockBase> treeBase = new Tree<TextBlockBase>(new TextBlock(0, templateString.Length - 1));
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
                    var commandWithTextBlock = new CommandWithTextBlock(startCommandLocation, endCommandLocation, templateString);
                    var textBlockTreeNode = textBlockTreeStack.Pop();
                    textBlockTreeNode.Data = commandWithTextBlock;
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
                if (treeNode.Any() && treeNode.Data.InnerContentRange != null)
                {
                    int numChildren = treeNode.Count();
                    int expectedNext = treeNode.Data.InnerContentRange.Value.startRange;
                    for (int childIndex = 0; childIndex < numChildren; childIndex++)
                    {
                        var child = treeNode[childIndex];
                        int childStart = child.Data.OverallRange.startRange;
                        if (expectedNext != childStart)
                        {
                            treeNode.Insert(childIndex, new Tree<TextBlockBase>(new TextBlock(expectedNext, childStart - 1)));
                            expectedNext = childStart;
                            numChildren++;
                        }
                        else
                            expectedNext = child.Data.OverallRange.endRange + 1;
                    }
                    int currentEnd = treeNode.Last().Data.OverallRange.endRange;
                    int expectedEnd = treeNode.Data.InnerContentRange.Value.endRange;
                    if (currentEnd < expectedEnd)
                        treeNode.Add(new Tree<TextBlockBase>(new TextBlock(currentEnd + 1, expectedEnd)));
                }
            }

        }

        private class CommandBase
        {

            protected StringTemplates StringTemplate;

            public CommandBase(StringTemplates stringTemplate)
            {
                StringTemplate = stringTemplate;
            }

            public virtual string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
            {
                return stringToTransform;
            }
        }

        private class SetVariableCommand : CommandBase
        {
            public SetVariableCommand(StringTemplates stringTemplate) : base(stringTemplate)
            {
            }

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
            {
                if (commandContent.Contains(','))
                {
                    var split = commandContent.Split(',').ToList();
                    string variableName = split[0];
                    string value = split[1];
                    variables[variableName] = value;
                }
                else
                {
                    if (variables.ContainsKey(commandContent))
                        variables.Remove(commandContent);
                }

                return stringToTransform; // If there are commands using the variable, the result will be processed again.
            }
        }

        /// <summary>
        /// Sets a variable to 1 if and only if the content text contains a string. This is used in conjunction with one or more IF commands in the content
        /// text set to that variable. 
        /// </summary>
        private class ContainsCommand : CommandBase
        {
            public ContainsCommand(StringTemplates stringTemplate) : base(stringTemplate)
            {
            }

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
            {
                var split = commandContent.Split(',').ToList();
                string variableName = split[0];
                string textToSearchFor = split[1];
                bool contained = stringToTransform.Contains(textToSearchFor);
                return StringTemplate.CreateSetVariableBlock(variableName, contained ? "1" : "0") + stringToTransform; // reprocess with this variable set at the beginning
            }
        }

        private class IfVariableCommand : CommandBase
        {
            public IfVariableCommand(StringTemplates stringTemplate) : base(stringTemplate)
            {
            }

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
            {
                var split = commandContent.Split(',').ToList();
                string variableName = split[0];
                string value = split[1];
                if (variables.ContainsKey(variableName))
                {
                    if (variables[variableName] == value)
                        return stringToTransform;
                    return "";
                }
                return StringTemplate.CreateIfBlock(variableName, value, stringToTransform); // The variable is not set yet, so we need to keep the if command until it is ready to be processed.
            }
        }

        private class VariableCommand : CommandBase
        {
            public VariableCommand(StringTemplates stringTemplate) : base(stringTemplate)
            {
            }

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
            {
                string variableName = commandContent;
                if (variables.ContainsKey(variableName))
                    return variables[variableName];
                return StringTemplate.CreateVariableBlock(variableName); // keep command intact by recreating it, so that it can be resolved based on variable at higher level in the tree (note that the higher levels in the tree are resolved afterward)
            }
        }

        private class ForCommand : CommandBase
        {
            public ForCommand(StringTemplates stringTemplate) : base(stringTemplate)
            {
            }

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
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
                    sb.Append(StringTemplate.Process(stringToTransform, variables));
                }
                variables.Remove(variableName);
                return sb.ToString();
            }
        }

        private class ReprocessCommand : CommandBase
        {
            public ReprocessCommand(StringTemplates stringTemplate) : base(stringTemplate)
            {
            }

            public override string TransformRange(string stringToTransform, string commandContent, Dictionary<string, string> variables)
            {
                int numCyclesLeft = 0;
                if (commandContent != null && commandContent != "")
                    numCyclesLeft = Int32.Parse(commandContent);
                // When there is exactly 1 cycle left, we unencode the command block. The result of this is that the contents will not be evaluated
                // until the last cycle.
                if (numCyclesLeft == 1)
                    stringToTransform = StringTemplate.EncodeCommandBlock(stringToTransform, unencode: true);
                if (numCyclesLeft > 0)
                    return StringTemplate.CreateReprocessBlock(stringToTransform, numCyclesLeft - 1); // we are going to delay processing; this allows external variables to be set
                return StringTemplate.Process(stringToTransform, variables);
            }
        }

        private Dictionary<string, CommandBase> _Transformers => new Dictionary<string, CommandBase>()
        {
            { "", new CommandBase(this) },
            { "set", new SetVariableCommand(this) },
            { "if", new IfVariableCommand(this) },
            { "var", new VariableCommand(this) },
            { "contains", new ContainsCommand(this) },
            { "for", new ForCommand(this) },
            { "reprocess", new ReprocessCommand(this) }
        };

        private Dictionary<string, CommandBase> Transformers => _Transformers;

        public string GetCommandTreeString(string str)
        {
            Tree<TextBlockBase> tree = GetCommandTree(str, BeginCommandOpenDelimeter, EndCommandOpenDelimeter, CloseDelimeter);
            var treeString = tree.GetTreeString(textBlock => textBlock.ToString(str));
            return treeString;
        }

        public string Process(string str, Dictionary<string, string> variables = null)
        {
            if (variables == null)
                variables = new Dictionary<string, string>();
            Tree<TextBlockBase> tree = GetCommandTree(str, BeginCommandOpenDelimeter, EndCommandOpenDelimeter, CloseDelimeter);
            // var treeString = tree.GetTreeString(textBlock => textBlock.ToString(str));
            string result = tree.StringProducer(
                treeNode =>
                {
                    string innerContent = treeNode.Data.GetInnerContent(str);
                    while (innerContent != null && innerContent.IndexOf(BeginCommandOpenDelimeter) >= 0)
                        innerContent = Process(innerContent, variables);
                    return innerContent;
                },
                (treeNode, innerContent) =>
                {
                    TextBlockBase block = treeNode.Data;
                    var command = block.GetCommand(str);
                    string commandContent = command.commandContent;
                    while (commandContent != null && commandContent.IndexOf(BeginCommandOpenDelimeter) >= 0)
                        commandContent = Process(commandContent, variables);
                    if (command.commandName == "")
                        return innerContent;
                    else
                    {
                        if (Transformers.ContainsKey(command.commandName))
                        {
                            string transformedString = Transformers[command.commandName].TransformRange(innerContent, commandContent, variables);
                            return transformedString;
                        }
                        else
                            throw new Exception("Transformer {command.commandName} not defined");
                    }
                }
                );
            return result;
        }
    }
}
