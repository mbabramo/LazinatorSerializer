using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    /// <summary>
    /// A record-like type with fewer parameters than properties. This would ordinarily be ignored, but the config file says to use it.
    /// </summary>
    public readonly struct MismatchedRecordLikeType
    {
        public int Age { get; }
        public string Name { get; }
        public bool SetByConstructor { get; }

        // we use all caps for parameters to make sure that isn't a problem
        public MismatchedRecordLikeType(int AGE, string NAME)
        {
            this.Age = AGE;
            this.Name = NAME;
            SetByConstructor = true;
        }
    }
}
