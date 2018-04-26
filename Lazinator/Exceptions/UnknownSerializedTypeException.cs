using System;

namespace Lazinator.Exceptions
{
    public class UnknownSerializedTypeException : Exception
    {
        private const string explanation = "Tried to deserialize type with UniqueID {0} but that type is not known, or it is an open generic type derived from the open generic type for the property.";
        
        public UnknownSerializedTypeException(int missingUniqueId) : this(String.Format(explanation, missingUniqueId))
        {

        }

        public UnknownSerializedTypeException(string explanation)
            : base(explanation)
        {
        }
    }
}
