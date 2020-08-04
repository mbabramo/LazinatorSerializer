using Lazinator.Core;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable struct, where the struct type is specified generically. 
    /// </summary>
    public partial struct WNullableStruct<T> : IWNullableStruct<T> where T : struct, ILazinator
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
