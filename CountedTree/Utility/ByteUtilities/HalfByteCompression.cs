using System;

namespace CountedTree.ByteUtilities
{
    public static class HalfByteCompression
    {

        public static byte GetValueAtUncompressedIndex(ReadOnlySpan<byte> compressed, int uncompressedIndex)
        {
            int compressedIndex = uncompressedIndex / 2;
            bool firstHalf = uncompressedIndex % 2 == 0;
            byte b = compressed[compressedIndex];
            byte first, second;
            Decompress(b, out first, out second);
            if (firstHalf)
                return first;
            else
                return second;
        }

        public static byte Compress(byte byteA, byte byteB)
        {
            if (byteA > 15 || byteB > 15)
                throw new Exception("Can only compress bytes from 0 to 15.");
            return (byte) (byteA | (byteB << 4));
        }

        public static void Decompress(byte b, out byte byteA, out byte byteB)
        {
            byteA = (byte) (b & (byte)(byte.MaxValue >> 4));
            byteB = (byte)(b >> 4);
        }

        public static byte[] Compress(ReadOnlySpan<byte> uncompressed)
        {
            int numCompressedSlots = uncompressed.Length / 2;
            if (uncompressed.Length % 2 == 1)
                numCompressedSlots++;
            byte[] compressed = new byte[numCompressedSlots];
            for (int i = 0; i < compressed.Length; i++)
            {
                int firstUncompressedIndex = i * 2;
                byte firstHalf = (byte)(uncompressed[firstUncompressedIndex]);
                byte secondHalf = 0;
                int secondUncompressedIndex = firstUncompressedIndex + 1;
                if (secondUncompressedIndex < uncompressed.Length)
                    secondHalf = (byte)(uncompressed[secondUncompressedIndex]);
                compressed[i] = Compress(firstHalf, secondHalf);
            }
            return compressed;
        }

        public static byte[] Decompress(byte[] compressed, bool evenNumberOfItems)
        {
            int numItems = compressed.Length * 2;
            if (!evenNumberOfItems)
                numItems--;
            byte[] uncompressed = new byte[numItems];
            int index = 0;
            foreach (byte c in compressed)
            {
                byte first, second;
                Decompress(c, out first, out second);
                uncompressed[index] = first;
                index++;
                if (index < numItems)
                    uncompressed[index] = second;
                index++;
            }
            return uncompressed;
        }
    }
}
