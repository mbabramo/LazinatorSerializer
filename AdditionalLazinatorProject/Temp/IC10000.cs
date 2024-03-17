
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace FuzzTests.n1
{
    [Lazinator((int)10000)]
    public interface IC10000
    {
        public long? p0 { get; set; }
        public float p1 { get; set; }
        public short p2 { get; set; }
        public string p3 { get; set; }
        public bool p4 { get; set; }
        public string? p5 { get; set; }
        public Guid p6 { get; set; }

    }
}
