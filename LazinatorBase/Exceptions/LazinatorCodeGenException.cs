using System;

namespace Lazinator.Exceptions
{
    public class LazinatorCodeGenException : Exception
    {
        private const string explanation = "An unknown error occurred during self-serialization code generation.";

        public LazinatorCodeGenException() : this(explanation)
        {

        }
        public LazinatorCodeGenException(string explanation)
            : base(explanation)
        {
        }

        public LazinatorCodeGenException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
