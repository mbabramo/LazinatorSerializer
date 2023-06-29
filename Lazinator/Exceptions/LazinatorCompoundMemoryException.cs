using System;

namespace Lazinator.Exceptions
{
    public class LazinatorCompoundMemoryException : Exception
    {
        private const string explanation = "This operation is not supported when Lazinator's memory storage is split into multiple blocks.";

        public LazinatorCompoundMemoryException() : this(explanation)
        {

        }

        public LazinatorCompoundMemoryException(string explanation)
            : base(explanation)
        {
        }

        public LazinatorCompoundMemoryException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
