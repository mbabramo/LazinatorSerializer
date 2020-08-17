using System.Diagnostics;
using System.Text;

namespace Lazinator.CodeDescription
{
    public class CodeStringBuilder
    {
        StringBuilder sb = new StringBuilder();
        public int SpacesPerTab = 4;
        public int IndentLevel = 0;
        public bool IsBeginningOfLine = true;
        public static bool AddLocationIndexComments = true;
        public static int StopAtLocationIndex = -1;
        public static int LocationIndex = 0;

        public override string ToString()
        {
            return sb.ToString();
        }

        private void IndentToCurrentLevel()
        {
            int spaces = SpacesPerTab * IndentLevel;
            for (int i = 0; i < spaces; i++)
                sb.Append(' ');
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
                if (LocationIndex == StopAtLocationIndex)
                {
                    Debugger.Break();
                }
                AppendHelper($"/*Location{LocationIndex++}*/");
            }
            AppendHelper(s);
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
    }
}
