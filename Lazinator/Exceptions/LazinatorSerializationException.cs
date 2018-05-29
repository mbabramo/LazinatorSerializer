using System;

namespace Lazinator.Exceptions
{
    public class LazinatorSerializationException : Exception
    {
        private const string explanation = "An unknown error occurred during serialization.";

        public LazinatorSerializationException() : this(explanation)
        {

        }

        public LazinatorSerializationException(string explanation)
            : base(explanation)
        {
        }

        public LazinatorSerializationException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
