using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public class EnumeratorPlus<T>
    {
        public IEnumerator<T> Enumerator;
        public bool MoreExist;

        public EnumeratorPlus(IEnumerator<T> enumerator)
        {
            Enumerator = enumerator;
            MoveNext();
        }

        public void MoveNext()
        {
            MoreExist = Enumerator.MoveNext();
        }

        public T GetNextOrDefault()
        {
            if (!MoreExist)
                return default(T);
            return Enumerator.Current;
        }

        public T GetNext()
        {
            if (!MoreExist)
                throw new Exception("No more exist.");
            return Enumerator.Current;
        }
    }
}
