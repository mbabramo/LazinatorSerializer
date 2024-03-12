using System;

namespace CountedTree.ByteUtilities
{
    public class BinaryWriter
    {
        public byte[] Bytes;
        private int numBytesWritten = 0;

        public BinaryWriter(int size)
        {
            Bytes = new byte[size];
        }

        public void WriteByte(byte b)
        {
            Bytes[numBytesWritten++] = b;
        }

        public void ShortenIfNecessary()
        {
            if (numBytesWritten < Bytes.Length)
            {
                byte[] replacement = new byte[numBytesWritten];
                Array.Copy(Bytes, replacement, numBytesWritten);
                Bytes = replacement;
            }
        }
    }
}
