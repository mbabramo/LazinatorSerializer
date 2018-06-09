using System;

namespace Lazinator.Exceptions
{
    public class UnexpectedDirtinessException : Exception
    {
        private const string explanation = "A Lazinator object was found to be dirty, even though it was not set as dirty.";

        public UnexpectedDirtinessException() : this(explanation)
        {
            
        }
        public UnexpectedDirtinessException(string explanation)
            : base(explanation)
        {
        }

        public UnexpectedDirtinessException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
