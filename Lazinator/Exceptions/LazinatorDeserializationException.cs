using System;

namespace Lazinator.Exceptions
{
    public class LazinatorDeserializationException : Exception
    {
        private const string explanation = "An unknown error occurred during deserialization.";

        public LazinatorDeserializationException() : this(explanation)
        {

        }

        public LazinatorDeserializationException(string explanation)
            : base(explanation)
        {
        }

        public LazinatorDeserializationException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
