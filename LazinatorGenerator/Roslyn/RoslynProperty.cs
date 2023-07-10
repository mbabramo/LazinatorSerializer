using Microsoft.CodeAnalysis;

namespace LazinatorCodeGen.Roslyn
{
    // there is a Roslyn class with this info, but it's internal

    public class RoslynProperty
    {
        public ISymbol Property;
        public ISymbol GetMethod;
        public ISymbol SetMethod;
    }
}
