using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableStruct<T> : ILazinatorWrapperNullableStruct<T> where T : struct, ILazinator
    {
        public bool IsNull => !HasValue;

        public T? AsNullableStruct
        {
            get => HasValue ? (T?)NonNullValue : null;
            set
            {
                if (value == null)
                {
                    HasValue = false;
                    NonNullValue = default;
                }
                else
                {
                    HasValue = true;
                    NonNullValue = (T)value;
                }
            }
        }

        public override string ToString()
        {
            return AsNullableStruct?.ToString() ?? "";
        }


    }
}
