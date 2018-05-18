using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples.Tuples
{
    public partial class RecordTuple : IRecordTuple
    {
        public MismatchedRecordLikeType MyMismatchedRecordLikeType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
