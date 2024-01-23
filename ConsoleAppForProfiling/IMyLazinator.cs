using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;
using ProjectForDebuggingGenerator;

namespace ProjectForDebuggingGenerator
{
    [Lazinator(21000)]
    public interface IMyLazinator
    {
        int MyInt { get; set; }
        string MyBool { get; set; }
        LazinatorList<WInt32> MyLazinatorList { get; set; }
        List<int> MyList { get; set; }
    }
}