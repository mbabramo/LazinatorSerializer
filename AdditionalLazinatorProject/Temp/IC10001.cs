
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10001)]
    public interface IC10001
    {
        public long? p7 { get; set; }
        public uint p8 { get; set; }
        public string p9 { get; set; }
        public int? p10 { get; set; }
        public long p11 { get; set; }

    }
}
