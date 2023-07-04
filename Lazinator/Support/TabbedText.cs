using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lazinator.Support
{
    /// <summary>
    /// A utility class that allows incrementing and decrementing of tabs.
    /// </summary>
    public static class TabbedText
    {
        public static StringBuilder AccumulatedText = new StringBuilder();

        public static int Tabs = 0;

        public static bool EnableOutput = true;

        public static void WriteLine(object value)
        {
            StringBuilder local = new StringBuilder();
            for (int i = 0; i < Tabs * 5; i++)
                local.Append(" ");
            local.Append(value);
            local.Append(Environment.NewLine);
            OutputAndAccumulate(local);
        }

        public static void WriteLine(string format, params object[] args)
        {
            Write(format, args);
            Write(Environment.NewLine);
            //StringBuilder local = new StringBuilder();
            //local.Append(Environment.NewLine);
            //OutputAndAccumulate(local);
        }

        public static void Write(string format, params object[] args)
        {
            StringBuilder local = new StringBuilder();
            for (int i = 0; i < Tabs * 5; i++)
                local.Append(" ");
            OutputAndAccumulate(local);
            WriteWithoutTabs(format, args);
        }

        public static void WriteWithoutTabs(string format, object[] args)
        {
            StringBuilder local = new StringBuilder();
            if (args != null && args.Any())
                local.Append(String.Format(format, args));
            else
                local.Append(format);
            OutputAndAccumulate(local);
        }

        static object StringBuilderAppend = new object(); // StringBuilder is not threadsafe

        private static void OutputAndAccumulate(StringBuilder builder)
        {
            string localString = builder.ToString();
            if (EnableOutput)
            {
                Debug.Write(localString);
            }
            lock (StringBuilderAppend)
            {
                AccumulatedText.Append(localString);
            }
        }

        public static void ResetAccumulated()
        {
            lock (StringBuilderAppend)
            {
                AccumulatedText = new StringBuilder();
            }
        }

        public static void WriteBytesVertically(IEnumerable<byte> bytes, bool includeIndices, IEnumerable<byte> previousForComparison = null, int startingIndex = 0)
        {
            var bytesList = bytes.ToList();
            if (includeIndices)
            {
                WriteVerticalStrings(Enumerable.Range(startingIndex, bytesList.Count).Select(x => (ulong)x));
                if (previousForComparison == null)
                    WriteLine("");
                else
                {
                    char[] differencesAsterisked = bytes.Zip(previousForComparison).Select(x => x.First == x.Second ? ' ' : '*').ToArray();
                    WriteLine(new string(differencesAsterisked));
                }
            }
            WriteVerticalStrings(bytes.Select(x => (ulong)x));
        }

        public static void WriteVerticalStrings(IEnumerable<ulong> numbers)
        {
            foreach (string horizontalLine in NumbersAsVerticalStrings(numbers))
            {
                WriteLine(horizontalLine);
            }
        }

        private static IEnumerable<string> NumbersAsVerticalStrings(IEnumerable<ulong> numbers)
        {
            // convert the numbers into character arrays, beginning with spaces for digits that aren't needed
            var charArrays = numbers
                .Select(x => x.ToString())
                .Select(x => x.ToCharArray(0, x.Length));
            var maxLength = charArrays.Max(x => x.Length);
            IEnumerable<char> WithLeadingSpaces(char[] charArray, int targetLength)
            {
                int numSpaces = targetLength - charArray.Length;
                for (int s = 0; s < numSpaces; s++)
                    yield return ' ';
                foreach (char c in charArray)
                    yield return c;
            }
            char[] CharArrayWithLeadingSpaces(char[] charArray, int targetLength) => WithLeadingSpaces(charArray, targetLength).ToArray();
            charArrays = charArrays.Select(x => CharArrayWithLeadingSpaces(x, maxLength));
            // Now, output the strings -- first taking all the first digits, then the second digits, etc.
            string s = new string(charArrays.First());
            for (int i = 0; i < maxLength; i++)
                yield return new string(charArrays.Select(x => x.Skip(i).First()).ToArray());
        }
    }
}