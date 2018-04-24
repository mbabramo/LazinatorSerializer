using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WrapperNullableStruct)]
    public interface ILazinatorWrapperNullableStruct<T> where T : struct, ILazinator
    {
        bool HasValue { get; set; }
        T NonNullValue { get; set; }
    }
}
