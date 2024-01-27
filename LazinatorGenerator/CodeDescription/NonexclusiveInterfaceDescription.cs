using System.Collections.Generic;
using System.Linq;
using LazinatorGenerator.AttributeClones;
using Microsoft.CodeAnalysis;
using LazinatorGenerator.CodeDescription;

namespace Lazinator.CodeDescription
{
    public class NonexclusiveInterfaceDescription
    {
        public List<PropertyDescription> Properties;
        LazinatorObjectDescription Container;
        NullableContext NullableContextSetting;

        public NonexclusiveInterfaceDescription()
        {

        }

        public NonexclusiveInterfaceDescription(LazinatorImplementingTypeInfo implementingTypeInfo, INamedTypeSymbol t, LazinatorObjectDescription container)
        {
            string typeName = LazinatorImplementingTypeInfo.TypeSymbolToString(t);
            if (!implementingTypeInfo.NonexclusiveInterfaces.Contains(typeName))
                throw new LazinatorCodeGenException("NonexclusiveLazinator must be applied to a nonexclusive interface.");
            Container = container;
            CloneNonexclusiveLazinatorAttribute nonexclusiveLazinatorAttribute = implementingTypeInfo.GetFirstAttributeOfType<CloneNonexclusiveLazinatorAttribute>(t);
            if (nonexclusiveLazinatorAttribute == null)
                throw new LazinatorCodeGenException("Expected NonexclusiveLazinator attribute.");
            if (implementingTypeInfo.PropertiesForType.ContainsKey(typeName))
            {
                Properties = implementingTypeInfo.PropertiesForType[typeName].Select(x => new PropertyDescription(x.Property, container, null, null, false)).ToList();
            }
        }
    }
}
