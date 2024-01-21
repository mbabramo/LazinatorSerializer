using Lazinator.Attributes;

namespace ProjectForDebuggingGenerator
{
    [Lazinator(1001)]
    internal interface IAnotherLazinator
    {
        string MyString { get; set; }
    }
}