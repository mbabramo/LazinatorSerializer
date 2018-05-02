using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazinatorAnalyzer.Roslyn;
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
        public List<PropertyDescription> PropertiesToDefineThisLevel;
        public List<string> GenericArgumentNames;
        public int TotalNumProperties;

        public ExclusiveInterfaceDescription()
        {

        }

        public ExclusiveInterfaceDescription(INamedTypeSymbol t, ObjectDescription container)
        {
            Container = container;
            var lazinatorAttribute = Container.CodeFiles.GetFirstAttributeOfType<CloneLazinatorAttribute>(t);
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
            var attributes = Container.CodeFiles.GetAttributesOfType<CloneUnofficiallyIncorporateInterfaceAttribute>(t);
            foreach (var a in attributes)
            {
                INamedTypeSymbol namedInterfaceType = Container.CodeFiles.LookupSymbol(a.OtherInterfaceFullyQualifiedTypeName);
                if (namedInterfaceType == null)
                    throw new LazinatorCodeGenException(
                        $"Unofficial type {a.OtherInterfaceFullyQualifiedTypeName} must exist and have a Lazinator attribute.");
                ExclusiveInterfaceDescription d = new ExclusiveInterfaceDescription(namedInterfaceType, Container);
                foreach (var p in d.PropertiesToDefineThisLevel)
                    if (!PropertiesToDefineThisLevel.Any(x => x.PropertyName == p.PropertyName))
                    {
                        p.PropertyAccessibility = a.Accessibility;
                        PropertiesToDefineThisLevel.Add(p);
                    }
            }
        }

        private void SetPropertiesIncludingInherited(INamedTypeSymbol interfaceSymbol)
        {
            var propertiesWithLevel = Container.CodeFiles.PropertiesForType[interfaceSymbol];
            var orderedPropertiesWithLevel = propertiesWithLevel.Select(x => new { propertyWithLevel = x, description = new PropertyDescription(x.Property, Container) })
                .OrderBy(x => x.description.PropertyType)
                .ThenBy(x => x.description.PropertyName).ToList();

            // A property that ends with "_Dirty" is designed to track dirtiness of another property. We will thus treat it specially.
            var dirtyPropertiesWithLevel = propertiesWithLevel.Where(x => x.Property.Name.EndsWith("_Dirty")).ToList();
            if (dirtyPropertiesWithLevel.Any(x => (x.Property.Type as INamedTypeSymbol)?.Name != "Boolean"))
                throw new LazinatorCodeGenException($"Property ending with _Dirty must be of type bool.");
            PropertiesIncludingInherited = new List<PropertyDescription>();
            PropertiesToDefineThisLevel = new List<PropertyDescription>();

            foreach (var orderedProperty in orderedPropertiesWithLevel)
            {
                if (orderedProperty.propertyWithLevel.LevelInfo ==
                    PropertyWithLevelInfo.Level.IsDefinedInLowerLevelInterface)
                {
                    orderedProperty.description.IsDefinedInLowerLevelInterface = true;
                    orderedProperty.description.DeriveKeyword = "override ";
                }

                if (!dirtyPropertiesWithLevel.Any(x => x.Property.Name == orderedProperty.propertyWithLevel.Property.Name))
                { // this is not itself a "_Dirty" property, though it may have a corresponding _Dirty property.
                    PropertiesIncludingInherited.Add(orderedProperty.description);
                    if (orderedProperty.propertyWithLevel.LevelInfo == PropertyWithLevelInfo.Level.IsDefinedThisLevel ||
                            (orderedProperty.propertyWithLevel.LevelInfo == PropertyWithLevelInfo.Level.IsDefinedInLowerLevelInterface 
                         && 
                            !Container.GetBaseObjectDescriptions().Any(x => !x.IsAbstract && x.PropertiesToDefineThisLevel.Any(y => y.FullyQualifiedTypeNameEncodable == orderedProperty.description.FullyQualifiedTypeNameEncodable))) ||
                        Container?.BaseLazinatorObject == null)
                    {
                        PropertiesToDefineThisLevel.Add(orderedProperty.description);
                    }
                }
            }

            // for each _dirty property, set TrackDirtinessNonSerialized on the corresponding property.
            foreach (var dirtyWithLevel in dirtyPropertiesWithLevel)
            {
                var match = PropertiesIncludingInherited.SingleOrDefault(x => x.PropertyName + "_Dirty" == dirtyWithLevel.Property.Name);
                if (match == null)
                    throw new LazinatorCodeGenException(
                        $"Property ending with _Dirty must have a corresponding property without the ending.");
                if (!match.IsNonSerializedType)
                    throw new LazinatorCodeGenException(
                        $"Property ending with _Dirty must correspond to a nonserialized property without the ending (not to a Lazinator or primitive type).");
                match.TrackDirtinessNonSerialized = true;
                match = PropertiesToDefineThisLevel.SingleOrDefault(x => x.PropertyName + "_Dirty" == dirtyWithLevel.Property.Name);
                if (match != null)
                    match.TrackDirtinessNonSerialized = true;
            }
        }
    }
}
