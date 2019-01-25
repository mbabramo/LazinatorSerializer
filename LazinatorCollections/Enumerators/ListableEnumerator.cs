using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Enumerators
{
    public struct ListableEnumerator<T> : IEnumerator<T> where T : ILazinator
    {
        ILazinatorListable<T> List;
        bool Started;
        bool Reverse;
        int Index;

        public ListableEnumerator(ILazinatorListable<T> list, bool reverse, long skip)
        {
            Started = false;
            Reverse = reverse;
            int skipAdjusted = (int)Math.Min(skip, list.Count);
            Index = reverse ? list.Count - skipAdjusted : skipAdjusted - 1;
            List = list;
        }

        public T Current
        {
            get
            {
                if (!Started)
                    throw new ArgumentException();
                return List[Index];
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (Reverse)
            {
                if (Index == 0)
                    return false;
                Index--;
            }
            else
            {
                if (Index == List.Count - 1)
                    return false;
                Index++;
            }
            Started = true;
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
