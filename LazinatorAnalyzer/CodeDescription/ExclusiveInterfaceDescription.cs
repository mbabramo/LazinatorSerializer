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
        public INamedTypeSymbol NamedTypeSymbol;
        public List<PropertyDescription> PropertiesIncludingInherited;
        public List<PropertyDescription> PropertiesToDefineThisLevel;
        public List<string> GenericArgumentNames;
        public int TotalNumProperties;
        public bool AutocloneAll;

        public ExclusiveInterfaceDescription()
        {

        }

        public ExclusiveInterfaceDescription(INamedTypeSymbol t, ObjectDescription container)
        {
            NamedTypeSymbol = t;
            Container = container;
            var lazinatorAttribute = Container.Compilation.GetFirstAttributeOfType<CloneLazinatorAttribute>(t);
            if (lazinatorAttribute == null)
                throw new LazinatorCodeGenException("Lazinator attribute is required for each interface implementing ILazinator, including inherited attributes.");
            var autocloneAllAttribute = Container.Compilation.GetFirstAttributeOfType<CloneAutocloneAllAttribute>(t);
            if (autocloneAllAttribute != null)
                AutocloneAll = true;
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
            NamedTypeSymbol = t;
            var attributes = Container.Compilation.GetAttributesOfType<CloneUnofficiallyIncorporateInterfaceAttribute>(t);
            foreach (var a in attributes)
            {
                INamedTypeSymbol namedInterfaceType = Container.Compilation.LookupSymbol(a.OtherInterfaceFullyQualifiedTypeName);
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
            var propertiesWithLevel = Container.Compilation.PropertiesForType[interfaceSymbol];
            foreach (var baseType in Container.GetAbstractBaseObjectDescriptions())
            {
                if (Container.Compilation.TypeToExclusiveInterface.ContainsKey(baseType.ILazinatorTypeSymbol))
                {
                    var baseExclusiveInterface = Container.Compilation.TypeToExclusiveInterface[baseType.ILazinatorTypeSymbol];
                    var additionalPropertiesWithLevel = Container.Compilation.PropertiesForType[baseExclusiveInterface];
                    foreach (var basePropertyWithLevel in additionalPropertiesWithLevel)
                    {
                        if (!propertiesWithLevel.Any(x => x.Property.MetadataName == basePropertyWithLevel.Property.MetadataName))
                            propertiesWithLevel.Add(new PropertyWithDefinitionInfo(basePropertyWithLevel.Property, PropertyWithDefinitionInfo.Level.IsDefinedLowerLevelButNotInInterface) { DerivationKeyword = "override " });
                    }
                }
            }
            var orderedPropertiesWithLevel = propertiesWithLevel.Select(x => new { propertyWithLevel = x, description = new PropertyDescription(x.Property, Container, x.DerivationKeyword, false) })
                .OrderByDescending(x => x.description.PropertyType == LazinatorPropertyType.PrimitiveType || x.description.PropertyType == LazinatorPropertyType.PrimitiveTypeNullable) // primitive properties are always first (but will only be redefined if defined abstractly below)
                .ThenBy(x => x.propertyWithLevel.LevelInfo == PropertyWithDefinitionInfo.Level.IsDefinedThisLevel) // defined this level later
                .ThenBy(x => x.description.PropertyName).ToList();
            var last = orderedPropertiesWithLevel.LastOrDefault();
            if (last != null)
                last.description.IsLast = true;

            // A property that ends with "_Dirty" is designed to track dirtiness of another property. We will thus treat it specially.
            var dirtyPropertiesWithLevel = propertiesWithLevel.Where(x => x.Property.Name.EndsWith("_Dirty")).ToList();
            if (dirtyPropertiesWithLevel.Any(x => (x.Property.Type as INamedTypeSymbol)?.Name != "Boolean"))
                throw new LazinatorCodeGenException($"Property ending with _Dirty must be of type bool.");
            PropertiesIncludingInherited = new List<PropertyDescription>();
            PropertiesToDefineThisLevel = new List<PropertyDescription>();

            foreach (var orderedProperty in orderedPropertiesWithLevel)
            {
                if (orderedProperty.propertyWithLevel.LevelInfo ==
                    PropertyWithDefinitionInfo.Level.IsDefinedInLowerLevelInterface)
                {
                    orderedProperty.description.IsDefinedInLowerLevelInterface = true;
                    orderedProperty.description.DerivationKeyword = "override ";
                }

                if (!dirtyPropertiesWithLevel.Any(x => x.Property.Name == orderedProperty.propertyWithLevel.Property.Name))
                { // this is not itself a "_Dirty" property, though it may have a corresponding _Dirty property.
                    PropertiesIncludingInherited.Add(orderedProperty.description);
                    if (orderedProperty.propertyWithLevel.LevelInfo == PropertyWithDefinitionInfo.Level.IsDefinedThisLevel ||
                            (
                             !Container.IsAbstract // if we have two consecutive abstract classes, we don't want to repeat the abstract properties
                             &&
                            !Container.GetBaseObjectDescriptions().Any(x => !x.IsAbstract && x.PropertiesToDefineThisLevel.Any(y => y.PropertyName == orderedProperty.description.PropertyName && y.FullyQualifiedTypeNameEncodable == orderedProperty.description.FullyQualifiedTypeNameEncodable)))
                        )
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
