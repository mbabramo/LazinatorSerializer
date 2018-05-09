using System;
using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples
{
    public partial class Example : IExample
    {
        public Example()
        {
            
        }

        public void LazinatorObjectVersionUpgrade(int oldFormatVersion)
        {
            if (oldFormatVersion < 3 && LazinatorObjectVersion >= 3)
            {
                MyNewString = "NEW " + MyOldString;
                MyOldString = null;
            }
        }
    }
}
