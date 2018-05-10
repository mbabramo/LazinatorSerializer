using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    interface ILazinatorWrapper<T>
    {
        T Value { get; set; }
        
        bool IsNull { get; }
    }

    // ReadOnlySpan etc. need their own interfaces. For now, we'll define only ReadOnlySpan<char>
}