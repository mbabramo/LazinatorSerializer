using Lazinator.Attributes;

namespace ProjectForDebuggingGenerator
{
    [Lazinator(652)]
    public interface IA
    {
        [SetterAccessibility("private")]
        int MyInt { get; }
    }
}