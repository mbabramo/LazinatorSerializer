using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

        private static void OutputAndAccumulate(StringBuilder builder)
        {
            string localString = builder.ToString();
            if (EnableOutput)
                Debug.Write(localString);
            AccumulatedText.Append(localString);
        }

        public static void ResetAccumulated()
        {
            AccumulatedText = new StringBuilder();
        }
    }
}