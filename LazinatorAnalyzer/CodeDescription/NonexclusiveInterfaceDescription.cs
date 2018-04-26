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
                throw new Exception("NonexclusiveLazinator must be applied to a nonexclusive interface.");
            Container = container;
            NonexclusiveLazinatorAttribute nonexclusiveLazinatorAttribute = fileSet.GetFirstAttributeOfType<NonexclusiveLazinatorAttribute>(t);
            if (nonexclusiveLazinatorAttribute == null)
                throw new Exception("Expected nonexclusive self-serialize attribute.");
            if (fileSet.PropertiesForType.ContainsKey(t))
                Properties = fileSet.PropertiesForType[t].Select(x => new PropertyDescription(x.property, container)).ToList();
        }
    }
}
