﻿using System;
using System.Reflection;
using System.Text;

namespace Lazinator.Support
{
    /// <summary>
    /// The Lazinator version information for this main project. The file in the Analyzer project with the same class name should also be changed.
    /// </summary>
    public static class LazinatorVersionInfo
    {
        public static readonly Version LazinatorVersion = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string LazinatorVersionString = LazinatorVersion.ToString();
        public static readonly byte[] LazinatorVersionBytes = Encoding.ASCII.GetBytes(LazinatorVersionString);
        public static int LazinatorIntVersion = 1; // this is encoded at the beginning of each Lazinator serialization
    }
}
