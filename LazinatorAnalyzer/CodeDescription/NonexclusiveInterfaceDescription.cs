﻿using System.Collections.Generic;
using System.Linq;
using LazinatorAnalyzer.AttributeClones;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;

namespace Lazinator.CodeDescription
{
    public class NonexclusiveInterfaceDescription
    {
        public List<PropertyDescription> Properties;
        ObjectDescription Container;

        public NonexclusiveInterfaceDescription()
        {

        }

        public NonexclusiveInterfaceDescription(LazinatorCompilation fileSet, INamedTypeSymbol t, ObjectDescription container)
        {
            string typeName = LazinatorCompilation.TypeSymbolToString(t);
            if (!fileSet.NonexclusiveInterfaces.Contains(typeName))
                throw new LazinatorCodeGenException("NonexclusiveLazinator must be applied to a nonexclusive interface.");
            Container = container;
            CloneNonexclusiveLazinatorAttribute nonexclusiveLazinatorAttribute = fileSet.GetFirstAttributeOfType<CloneNonexclusiveLazinatorAttribute>(t);
            if (nonexclusiveLazinatorAttribute == null)
                throw new LazinatorCodeGenException("Expected nonexclusive self-serialize attribute.");
            if (fileSet.PropertiesForType.ContainsKey(typeName))
            {
                Properties = fileSet.PropertiesForType[typeName].Select(x => new PropertyDescription(x.Property, container, null, null, false)).ToList();
            }
        }
    }
}
