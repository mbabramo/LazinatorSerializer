using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapper<T> : ILazinatorWrapper<T>
    {
        public T Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
