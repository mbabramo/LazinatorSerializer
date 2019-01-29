using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Exceptions
{
    public static class ThrowHelper
    {
        public static void ThrowCannotUpdateStoredBuffer() =>
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");

        public static void ThrowFormatException() =>
                                throw new FormatException("Wrong Lazinator type initialized.");
    }
}
