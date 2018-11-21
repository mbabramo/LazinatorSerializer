using System;
using System.Text;

namespace LazinatorCodeGen.Roslyn
{
    // Analyzer project: Must also change LazinatorVersionInfo in main Lazinator project
    public static class LazinatorVersionInfo
    {
        public static readonly Version LazinatorVersion = new Version(0, 1, 0, 321);
        public static readonly string LazinatorVersionString = LazinatorVersion.ToString();
        public static readonly byte[] LazinatorVersionBytes = Encoding.ASCII.GetBytes(LazinatorVersionString);
        public static int LazinatorIntVersion = 1; // this is encoded at the beginning of each Lazinator serialization
    }
}
