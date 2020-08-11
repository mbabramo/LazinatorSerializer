using System;

namespace Lazinator.Exceptions
{
    public class UnsetNonnullableLazinatorException : Exception
    {
        private const string explanation = "Nonnullable Lazinator property must be set before it is accessed.";

        public UnsetNonnullableLazinatorException() : this(explanation)
        {

        }

        public UnsetNonnullableLazinatorException(string explanation)
            : base(explanation)
        {
        }

        public UnsetNonnullableLazinatorException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
