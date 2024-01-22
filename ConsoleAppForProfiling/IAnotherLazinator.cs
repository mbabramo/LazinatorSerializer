using Lazinator.Attributes;

namespace ProjectForDebuggingGenerator
{
    [Lazinator(21001)]
    internal interface IAnotherLazinator
    {
        string MyString { get; set; }
    }
}