using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.SpanInDotNetList)]
    public interface ISpanInDotNetList
    {
        // ReadOnlySpan can't be directly in a list, but it can be indirectly.
        List<SpanAndMemory> SpanList { get; set; }
        int SomeInt { get; set; }
    }
}