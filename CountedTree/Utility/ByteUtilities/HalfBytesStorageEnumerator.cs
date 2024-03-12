using System.Collections;
using System.Collections.Generic;

namespace CountedTree.ByteUtilities
{
    public class HalfBytesStorageEnumerator : IEnumerator<byte>
    {
        public int index = -1;
        public HalfBytesStorage Storage;

        public HalfBytesStorageEnumerator(HalfBytesStorage storage)
        {
            Storage = storage;
        }


        public byte Current
        {
            get
            {
                return Storage[index];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Storage[index];
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            index++;
            return index <= Storage.Count - 1;
        }

        public void Reset()
        {
            index = -1;
        }

    }
}
