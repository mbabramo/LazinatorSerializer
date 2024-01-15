using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.Support
{
    public class FakeDateTimeNow : IDateTimeNow
    {
        public DateTime Value { get; set; } = new DateTime(2024, 1, 1);
    }
}
