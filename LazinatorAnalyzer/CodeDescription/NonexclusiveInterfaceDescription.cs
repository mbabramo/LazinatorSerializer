using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazinatorCodeGen.AttributeClones;
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
            if (!fileSet.NonexclusiveInterfaces.Contains(t))
                throw new LazinatorCodeGenException("NonexclusiveLazinator must be applied to a nonexclusive interface.");
            Container = container;
            CloneNonexclusiveLazinatorAttribute nonexclusiveLazinatorAttribute = fileSet.GetFirstAttributeOfType<CloneNonexclusiveLazinatorAttribute>(t);
            if (nonexclusiveLazinatorAttribute == null)
                throw new LazinatorCodeGenException("Expected nonexclusive self-serialize attribute.");
            if (fileSet.PropertiesForType.ContainsKey(t))
            {
                Properties = fileSet.PropertiesForType[t].Select(x => new PropertyDescription(x.Property, container, null, false)).ToList();
            }
        }
    }
}
