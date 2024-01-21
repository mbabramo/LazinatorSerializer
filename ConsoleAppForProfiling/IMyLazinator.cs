using Lazinator.Attributes;

namespace ConsoleAppForProfiling
{
    [Lazinator(1000)]
    public interface IMyLazinator
    {
        int MyInt { get; set; }
        string MyBool { get; set; }
        List<int> MyList { get; set; }
    }
}