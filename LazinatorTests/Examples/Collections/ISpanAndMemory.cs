using System;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.SpanAndMemory)]
    public interface ISpanAndMemory
    {
        // Note: In general, the types to use are ReadOnlySpan<T> if it's read-only and Memory<T> otherwise.
        // Where T is byte, the ReadOnlyMemory<byte> class is also efficient.
        // The ReadOnlySpan<T> types will have a backing field of type ReadOnlyMemory<byte>. When the get
        // accessor is called, this backing field will be set from the existing Lazinator memory. Then,
        // the backing field will be cast to ReadOnlySpan<T>, so no memory needs to be
        // copied. Thus, ReadOnlySpan<T> is very memory efficient. When the set accessor is called, changing 
        // the contents of the read-only memory, then the memory will be copied. 
        // With Memory<T>, a Memory<T> is also the backing field. When first accessed, however, the memory will be 
        // copied. After that, particular indices may be changed at any time. If the set accessor is called, then
        // the value will be set to the specified value without copying. Thus, Memory<T> may be faster than ReadOnlySpan<T>
        // if you anticipate frequently changing the entire value of the span/memory.
        // With Memory<T>, when serialization occurs, if access has occurred, all of the
        // memory will be serialized from the backing field. This is thus an efficient approach, given the constraint
        // that the user must be allowed to change the memory.
        // The problem with ReadOnlyMemory<notbyte> implementation is that the memory from storage is stored
        // as ReadOnlyMemory<byte>, and ReadOnlyMemory<byte> cannot be cast to ReadOnlyMemory<notbyte>. 
        // (In contrast, Span<notbyte> can be cast into Span<byte>.) Thus, these implementations copy
        // the child storage, much as Memory<T> does. Thus, it's no less efficient than Memory<notbyte>, but
        // we don't get any advantage from efficiency in having it read-only. To obtain that benefit, use the
        // ReadOnlySpan<notbyte> type.
        // Also, Lazinator allows a nullable Memory<T>? and a nullable ReadOnlyMemory<T>? but not a nullable ReadOnlySpan<T>?.
        // It might be possible to support this scenario with some additional work.
        // Finally, note that Lazinator does not support Span<T> properties. The reason is that Memory<T> would need
        // to be used as the backing property, but if the property type were Span<T>, then the setter would need
        // to copy all of the bytes from the Span<T> to Memory<T>, since there is no conversion from Span<T> to
        // Memory<T>. Thus, it is better to just use Memory<T>, and then access the Span property,
        // thus making it possible at least to set the memory from another Memory<T> without copying it.
        // If at some point we implement Lazinator ref structs, then we could include a Span<T> in
        // a ref struct, using Span<T> as the backing field.
        ReadOnlySpan<long> MyReadOnlySpanLong { get; set; }
        ReadOnlySpan<byte> MyReadOnlySpanByte { get; set; }
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