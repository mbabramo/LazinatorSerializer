using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.Support
{
    public class RealDateTimeNow : IDateTimeNow
    {
        public DateTime Value => DateTime.Now;
    }
}
