﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazinatorGenerator.Roslyn;
using LazinatorGenerator.AttributeClones;
using Microsoft.CodeAnalysis;
using LazinatorCodeGen.Roslyn;
using LazinatorGenerator.CodeDescription;

namespace Lazinator.CodeDescription
{
    public class ExclusiveInterfaceDescription
    {
        public int UniqueID;
        public int Version;
        public LazinatorObjectDescription Container;
        public INamedTypeSymbol NamedTypeSymbol;
        public INamedTypeSymbol BaseType => NamedTypeSymbol.BaseType;
        public List<PropertyDescription> PropertiesIncludingInherited;
        public List<PropertyDescription> PropertiesToDefineThisLevel;
        public List<PropertyDescription> PropertiesInherited;
        public List<string> GenericArgumentNames;
        public bool NullableModeEnabled;
        public int TotalNumProperties;
        public bool AutoChangeParentAll;
        public bool IsUnofficialInterface;
        public Compilation Compilation;

        public ExclusiveInterfaceDescription()
        {

        }

        public ExclusiveInterfaceDescription(Compilation compilation, INamedTypeSymbol t, bool nullableModeEnabled, LazinatorObjectDescription container, bool isUnofficialInterface = false)
        {
            Compilation = compilation;
            NamedTypeSymbol = t;
            NullableModeEnabled = nullableModeEnabled;
            IsUnofficialInterface = isUnofficialInterface;
            Container = container;
            var lazinatorAttribute = Container.ImplementingTypeInfo.GetFirstAttributeOfType<CloneLazinatorAttribute>(t);
            if (lazinatorAttribute == null)
                throw new LazinatorCodeGenException("Lazinator attribute is required for each interface implementing ILazinator, including inherited interfaces.");
            UniqueID = lazinatorAttribute.UniqueID;
            Version = lazinatorAttribute.Version;

            var genericArguments = t.TypeArguments.ToList();
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();

            SetPropertiesIncludingInherited(t);

            TotalNumProperties = PropertiesIncludingInherited.Count();
        }

        private List<PropertyDescription> GetUnofficialProperties(INamedTypeSymbol t)
        {
            if (IsUnofficialInterface)
                return new List<PropertyDescription>(); // unofficial interfaces can't incorporate other unofficial interfaces
            var attributes = Container.ImplementingTypeInfo.GetAttributesOfType<CloneUnofficiallyIncorporateInterfaceAttribute>(t);
            List<PropertyDescription> propertiesToPossiblyUnofficiallyIncorporate = new List<PropertyDescription>();
            foreach (var a in attributes)
            {
                INamedTypeSymbol namedInterfaceType = Container.ImplementingTypeInfo.LookupSymbol(a.OtherInterfaceFullyQualifiedTypeName);
                if (namedInterfaceType == null)
                    throw new LazinatorCodeGenException(
                        $"Unofficial type {a.OtherInterfaceFullyQualifiedTypeName} must exist and have a Lazinator attribute.");
                var containerToUse = Container.GetBaseLazinatorObjects().LastOrDefault(x => x.InterfaceTypeSymbol != null && x.InterfaceTypeSymbol.GetKnownAttribute<CloneUnofficiallyIncorporateInterfaceAttribute>()?.OtherInterfaceFullyQualifiedTypeName == a.OtherInterfaceFullyQualifiedTypeName) ?? Container;
                ExclusiveInterfaceDescription d = new ExclusiveInterfaceDescription(Container.ImplementingTypeInfo.Compilation, namedInterfaceType, NullableModeEnabled, containerToUse, true);
                foreach (var property in d.PropertiesToDefineThisLevel)
                {
                    property.PropertyAccessibility = a.Accessibility;
                    propertiesToPossiblyUnofficiallyIncorporate.Add(property);
                }
            }

            return propertiesToPossiblyUnofficiallyIncorporate;
        }

        private void SetPropertiesIncludingInherited(INamedTypeSymbol interfaceSymbol)
        {
            List<PropertyWithDefinitionInfo> propertiesWithLevel = Container.ImplementingTypeInfo.PropertiesForType[LazinatorImplementingTypeInfo.TypeSymbolToString(interfaceSymbol)];
            foreach (var unofficialProperty in GetUnofficialProperties(interfaceSymbol))
            {
                if (!propertiesWithLevel.Any(x => x.Property.MetadataName == unofficialProperty.PropertySymbol.MetadataName))
                    propertiesWithLevel.Add(new PropertyWithDefinitionInfo(unofficialProperty.PropertySymbol, PropertyWithDefinitionInfo.Level.IsDefinedThisLevel) { PropertyAccessibility = unofficialProperty.PropertyAccessibility });
            }
            foreach (var baseType in Container.GetBaseLazinatorObjects())
            {
                List<IPropertySymbol> additionalProperties;
                bool baseTypeIsIndexed = Container.ImplementingTypeInfo.TypeToExclusiveInterface.ContainsKey(LazinatorImplementingTypeInfo.TypeSymbolToString(baseType.ILazinatorTypeSymbol));
                if (baseTypeIsIndexed)
                {
                    var baseExclusiveInterface = Container.ImplementingTypeInfo.TypeToExclusiveInterface[LazinatorImplementingTypeInfo.TypeSymbolToString(baseType.ILazinatorTypeSymbol)];
                    additionalProperties = Container.ImplementingTypeInfo.PropertiesForType[baseExclusiveInterface].Select(x => x.Property).ToList();
                }
                else
                {
                    additionalProperties = new List<IPropertySymbol>();
                }
                foreach (var baseProperty in additionalProperties)
                {
                    if (!propertiesWithLevel.Any(x => x.Property.MetadataName == baseProperty.MetadataName))
                        propertiesWithLevel.Add(new PropertyWithDefinitionInfo(baseProperty, PropertyWithDefinitionInfo.Level.IsDefinedLowerLevelButNotInInterface) { DerivationKeyword = "override " });
                }
                // now, unofficial properties
                var baseUnofficialProperties = GetUnofficialProperties(baseType.InterfaceTypeSymbol);
                foreach (var unofficialProperty in baseUnofficialProperties)
                {
                    if (!propertiesWithLevel.Any(x => x.Property.MetadataName == unofficialProperty.PropertySymbol.MetadataName))
                        propertiesWithLevel.Add(new PropertyWithDefinitionInfo(unofficialProperty.PropertySymbol, PropertyWithDefinitionInfo.Level.IsDefinedLowerLevelButNotInInterface) { DerivationKeyword = "override ", PropertyAccessibility = unofficialProperty.PropertyAccessibility });
                }
            }

            var orderedPropertiesWithLevel = propertiesWithLevel.Select(x => new { propertyWithLevel = x, description = new PropertyDescription(x.Property, Container, x.DerivationKeyword, x.PropertyAccessibility, false) })
                .OrderByDescending(x => x.description.PropertyType == LazinatorPropertyType.PrimitiveType || x.description.PropertyType == LazinatorPropertyType.PrimitiveTypeNullable) // primitive properties are always first (but will only be redefined if defined abstractly below)
                //.ThenBy(x => x.propertyWithLevel.LevelInfo == PropertyWithDefinitionInfo.Level.IsDefinedThisLevel)
                .ThenBy(x => x.description.RelativeOrder)
                .ThenBy(x => x.description.PropertyName).ToList(); /* note that ordering is alphabetical across levels, except that this level is last -- alternative would be to do closest levels first and then stick with order within level */
            var last = orderedPropertiesWithLevel.LastOrDefault();
            if (last != null)
                last.description.IsLast = true;

            // A property that ends with "_Dirty" is designed to track dirtiness of another property. We will thus treat it specially.
            var dirtyPropertiesWithLevel = propertiesWithLevel.Where(x => x.Property.Name.EndsWith("_Dirty")).ToList();
            if (dirtyPropertiesWithLevel.Any(x => (x.Property.Type as INamedTypeSymbol)?.Name != "Boolean"))
                throw new LazinatorCodeGenException($"Property ending with _Dirty must be of type bool.");
            PropertiesIncludingInherited = new List<PropertyDescription>();
            PropertiesToDefineThisLevel = new List<PropertyDescription>();
            PropertiesInherited = new List<PropertyDescription>();

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
                            !Container.GetBaseLazinatorObjects().Any(x => !x.IsAbstract && x.PropertiesToDefineThisLevel.Any(y => y.PropertyName == orderedProperty.description.PropertyName)))
                        )
                    {
                        PropertiesToDefineThisLevel.Add(orderedProperty.description);
                    }
                    else
                    {
                        PropertiesInherited.Add(orderedProperty.description);
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
                if (!match.IsNonLazinatorType)
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
