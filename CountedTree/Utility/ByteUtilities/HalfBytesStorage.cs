using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.ByteUtilities
{
    public partial class HalfBytesStorage : IHalfBytesStorage, IEnumerable<byte>
    {

        public HalfBytesStorage(bool twoPerByte, byte[] storage)
        {
            TwoPerByte = twoPerByte;
            EvenNumber = storage.Length % 2 == 0;
            if (TwoPerByte)
                Storage = HalfByteCompression.Compress(storage);
            else
                Storage = storage;
        }

        public ReadOnlySpan<byte> AsUncompressed()
        {
            if (TwoPerByte)
                return AsEnumerable().ToArray();
            else
                return Storage.Span;
        }

        public IEnumerable<byte> AsEnumerable()
        {
            IEnumerator<byte> enumerator = GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public int Count
        {
            get
            {
                if (TwoPerByte)
                    return Storage.Length * 2 - (EvenNumber ? 0 : 1);
                else
                    return Storage.Length;
            }
        }

        public byte this[int index]
        {
            get
            {
                if (TwoPerByte)
                    return HalfByteCompression.GetValueAtUncompressedIndex(Storage.Span, index);
                else
                    return Storage.Span[index];
            }
        }

        private byte GetItem(int index)
        {
            return this[index];
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return new HalfBytesStorageEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new HalfBytesStorageEnumerator(this);
        }
    }
}
