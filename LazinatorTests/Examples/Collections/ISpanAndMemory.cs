using System;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.SpanAndMemory)]
    public interface ISpanAndMemory
    {
        // Note: In general, the types to use are ReadOnlySpan<T> if it's read-only and Memory<T> otherwise.
        // Where T is byte, the ReadOnlyMemory<byte> class is also efficient.
        // The problem with ReadOnlyMemory<notbyte> implementation is that the memory from storage is stored
        // as ReadOnlyMemory<byte>, and ReadOnlyMemory<byte> cannot be cast to ReadOnlyMemory<notbyte>. 
        // (In contrast, Span<notbyte> can be cast into Span<byte>.) Thus, these implementations convert
        // the child storage one element at a time.
        // Meanwhile, the Memory<T> implementations also convert memory, but this may be needed where 
        // the memory is not read-only. Also, Lazinator allows a nullable Memory<T>? and a nullable
        // ReadOnlyMemory<T>? but not a nullable ReadOnlySpan<T>?.
        // Finally, note that we do not support Span<T> properties. The reason is that Memory<T> would need
        // to be used as the backing property, but if the property type were Span<T>, then the setter would need
        // to copy all of the bytes from the Span<T> to Memory<T>, since there is no conversion from Span<T> to
        // Memory<T>. Thus, it is better to just use Memory<T>, thus making it possible at least to set the
        // memory from another Memory<T> without copying it.
        // If at some point we implement Lazinator ref structs, then we could include a Span<T> in
        // a ref struct, using Span<T> as the backing field.
        ReadOnlySpan<long> MyReadOnlySpanLong { get; set; }
        ReadOnlySpan<byte> MyReadOnlySpanByte { get; set; }
        ReadOnlySpan<DateTime> MyReadOnlySpanDateTime { get; set; }
        ReadOnlySpan<char> MyReadOnlySpanChar { get; set; }
        Memory<byte> MyMemoryByte { get; set; }
        Memory<byte>? MyNullableMemoryByte { get; set; }
        Memory<int> MyMemoryInt { get; set; }
        Memory<int>? MyNullableMemoryInt { get; set; }
        ReadOnlyMemory<byte> MyReadOnlyMemoryByte { get; set; }
        // the following are permissible, but not encouraged.
        ReadOnlyMemory<char> MyReadOnlyMemoryChar { get; set; }
        ReadOnlyMemory<int> MyReadOnlyMemoryInt { get; set; }
        ReadOnlyMemory<int>? MyNullableReadOnlyMemoryInt { get; set; }
    }
}