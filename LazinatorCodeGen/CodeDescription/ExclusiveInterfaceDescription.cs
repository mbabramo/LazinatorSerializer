using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazinatorCodeGen.AttributeClones;
using Microsoft.CodeAnalysis;

namespace Lazinator.CodeDescription
{
    public class ExclusiveInterfaceDescription
    {
        public int UniqueID;
        public int Version;
        public ObjectDescription Container;
        public List<PropertyDescription> PropertiesIncludingInherited;
        public List<PropertyDescription> PropertiesThisLevel;
        public List<string> GenericArgumentNames;
        public int TotalNumProperties;

        public ExclusiveInterfaceDescription()
        {

        }

        public ExclusiveInterfaceDescription(INamedTypeSymbol t, ObjectDescription container)
        {
            Container = container;
            var lazinatorAttribute = Container.CodeFiles.GetFirstAttributeOfType<LazinatorAttribute>(t);
            if (lazinatorAttribute == null)
                throw new Exception("Lazinator attribute is required for each interface implementing ILazinator, including inherited attributes.");
            UniqueID = lazinatorAttribute.UniqueID;
            Version = lazinatorAttribute.Version;

            var genericArguments = t.TypeArguments.ToList();
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();

            SetPropertiesIncludingInherited(t);

            UnofficiallyIncorporateOtherProperties(t);

            TotalNumProperties = PropertiesIncludingInherited.Count();
        }

        private void UnofficiallyIncorporateOtherProperties(INamedTypeSymbol t)
        {
            var attributes = Container.CodeFiles.GetAttributesOfType<UnofficiallyIncorporateInterfaceAttribute>(t);
            foreach (var a in attributes)
            {
                INamedTypeSymbol namedInterfaceType = Container.CodeFiles.LookupSymbol(a.OtherInterfaceFullyQualifiedTypeName);
                ExclusiveInterfaceDescription d = new ExclusiveInterfaceDescription(namedInterfaceType, Container);
                foreach (var p in d.PropertiesThisLevel)
                    if (!PropertiesThisLevel.Any(x => x.PropertyName == p.PropertyName))
                    {
                        p.PropertyAccessibility = a.Accessibility;
                        PropertiesThisLevel.Add(p);
                    }
            }
        }

        private void SetPropertiesIncludingInherited(INamedTypeSymbol interfaceSymbol)
        {
            var propertiesWithLevel = Container.CodeFiles.PropertiesForType[interfaceSymbol];
            var orderedPropertiesWithLevel = propertiesWithLevel.Select(x => new { propertyWithLevel = x, description = new PropertyDescription(x.property, Container) })
                .OrderBy(x => x.description.PropertyType)
                .ThenBy(x => x.description.PropertyName).ToList();

            var dirtyPropertiesWithLevel = propertiesWithLevel.Where(x => x.property.Name.EndsWith("_Dirty")).ToList();
            if (dirtyPropertiesWithLevel.Any(x => (x.property.Type as INamedTypeSymbol)?.Name != "Boolean"))
                throw new LazinatorCodeGenException($"Property ending with _Dirty must be of type bool.");
            PropertiesIncludingInherited = new List<PropertyDescription>();
            PropertiesThisLevel = new List<PropertyDescription>();

            foreach (var orderedProperty in orderedPropertiesWithLevel)
            {
                if (!dirtyPropertiesWithLevel.Any(x => x.property.Name == orderedProperty.propertyWithLevel.property.Name))
                {
                    PropertiesIncludingInherited.Add(orderedProperty.description);
                    if (orderedProperty.propertyWithLevel.isThisLevel || Container?.BaseLazinatorObject == null)
                        PropertiesThisLevel.Add(orderedProperty.description);
                }
            }

            foreach (var dirtyWithLevel in dirtyPropertiesWithLevel)
            {
                var match = PropertiesIncludingInherited.SingleOrDefault(x => x.PropertyName + "_Dirty" == dirtyWithLevel.property.Name);
                if (match == null)
                    throw new LazinatorCodeGenException(
                        $"Property ending with _Dirty must have a corresponding property without the ending.");
                if (!match.IsNonSerializedType)
                    throw new LazinatorCodeGenException(
                        $"Property ending with _Dirty must correspond to a nonserialized property without the ending (not to a Lazinator or primitive type).");
                match.TrackDirtinessNonSerialized = true;
                match = PropertiesThisLevel.SingleOrDefault(x => x.PropertyName + "_Dirty" == dirtyWithLevel.property.Name);
                if (match != null)
                    match.TrackDirtinessNonSerialized = true;
            }
        }
    }
}
