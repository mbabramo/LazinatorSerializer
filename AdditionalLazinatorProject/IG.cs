using Lazinator.Attributes;
using Lazinator.Core;

namespace ProjectForDebuggingGenerator
{
    [Lazinator(658)]
    public interface IG<T> where T : ILazinator
    {
        List<T> MyList { get; set; }
    }
}