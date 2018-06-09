using System;

namespace Lazinator.Exceptions
{
    public class MovedLazinatorException : Exception
    {
        private const string explanation = "A Lazinator object that already has a parent set cannot be set as a property on another object. Consider setting a clone of the object or carefully use the AllowMovedAttribute.";

        public MovedLazinatorException() : this(explanation)
        {

        }

        public MovedLazinatorException(string explanation)
            : base(explanation)
        {
        }

        public MovedLazinatorException(string explanation, Exception inner)
            : base(explanation, inner)
        {
        }
    }
}
