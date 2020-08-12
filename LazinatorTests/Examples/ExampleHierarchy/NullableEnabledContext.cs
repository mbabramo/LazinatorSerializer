using LazinatorCollections;
using LazinatorTests.Examples.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    public partial class NullableEnabledContext : INullableEnabledContext
    {
        public NullableEnabledContext()
        {

        }

        public Example[] NonNullableArray { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Example[] NullableArray { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public (Example, int) ValueTupleWithNonNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public (Example, int) ValueTupleWithNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Tuple<Example, int> RegularTupleWithNonNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Tuple<Example, int> RegularTupleWithNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<int, Example> DictionaryWithNonNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<int, Example> DictionaryWithNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorList<Example> LazinatorListNonNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorList<Example> LazinatorListNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NullableString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NonNullableString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReadOnlySpan<byte> ByteReadOnlySpan { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Memory<byte> ByteMemory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReadOnlyMemory<byte> ByteReadOnlyMemory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Memory<byte>? ByteMemoryNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReadOnlyMemory<byte>? ByteReadOnlyMemoryNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Queue<Example> NonNullableQueue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Queue<Example> NullableQueue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Stack<Example> NonNullableStack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Stack<Example> NullableStack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int MyInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? MyNullableInt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ExampleStructWithoutClass MyStruct { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
