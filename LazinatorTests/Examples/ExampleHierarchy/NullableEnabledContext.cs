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

        public RecordLikeStruct NonNullableRecordLikeStruct { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RecordLikeStruct? NullableRecordLikeStruct { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RecordLikeClass NonNullableRecordLikeClass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RecordLikeClass NullableRecordLikeClass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
