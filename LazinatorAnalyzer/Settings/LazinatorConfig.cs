using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using LazinatorCodeGen.Roslyn;

namespace LazinatorAnalyzer.Settings
{

    public class LazinatorConfig
    {
        public Dictionary<string, string> InterchangeMappings;

        public string GetInterchangeTypeName(INamedTypeSymbol type)
        {
            string fullyQualifiedName = type.GetFullyQualifiedName();
            if (InterchangeMappings.ContainsKey(fullyQualifiedName))
                return InterchangeMappings[fullyQualifiedName];
            return null;
        }
    }
}
