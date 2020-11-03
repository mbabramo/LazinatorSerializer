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

        public static void ThrowVersioningDisabledException(string nameIncludingGenerics) => throw new LazinatorSerializationException($"Lazinator versioning disabled for {nameIncludingGenerics}.");

        public static void ThrowTooLargeException(int maxValue = byte.MaxValue /* DEBUG -- eliminate default */) => throw new LazinatorSerializationException(tooLargeMessage(maxValue));

        private static string tooLargeMessage(int maxValue) => $"Contents exceeded maximum length of {maxValue} bytes.";

        internal static void ThrowChildStorageMissingException() => throw new LazinatorSerializationException("Internal error. Child storage missing.");

        public static void ThrowUnsetNonnullableLazinatorException() => throw new UnsetNonnullableLazinatorException();
    }
}
