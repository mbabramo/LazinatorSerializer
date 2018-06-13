using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Support
{
    // also change file in CodeGen with same class name
    public static class LazinatorVersionInfo
    {
        public static readonly Version LazinatorVersion = new Version(0, 1, 0, 104);
        public static readonly string LazinatorVersionString = LazinatorVersion.ToString();
        public static readonly byte[] LazinatorVersionBytes = Encoding.ASCII.GetBytes(LazinatorVersionString);
        public static int LazinatorIntVersion = 1; // this is encoded at the beginning of each Lazinator serialization
    }
}
