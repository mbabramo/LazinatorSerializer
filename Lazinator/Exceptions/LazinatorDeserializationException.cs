using System;

namespace Lazinator.Exceptions
{
    public class LazinatorDeserializationException : Exception
    {
        private const string explanation = "An unknown error occurred during deserialization.";

        public LazinatorDeserializationException() : this(explanation)
        {

        }

        static void ThrowNoDeserializationFactory() => throw new LazinatorDeserializationException(
            "The object could not be deserialized, because the deserialization factory has not been set. Assign the DeserializationFactory property before deserialization.");

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
