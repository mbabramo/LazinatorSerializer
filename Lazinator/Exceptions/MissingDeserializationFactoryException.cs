using System;

namespace Lazinator.Exceptions
{
    public class MissingDeserializationFactoryException : Exception
    {
        private const string explanation = "Deserialization requires that the DeserializationFactory property be set to a valid DeserializationFactory.";

        public MissingDeserializationFactoryException() : this(explanation)
        {

        }
        public MissingDeserializationFactoryException(string explanation)
            : base(explanation)
        {
        }

        public MissingDeserializationFactoryException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
