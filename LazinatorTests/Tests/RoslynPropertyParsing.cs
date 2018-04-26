using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using LazinatorTests.Examples.Collections;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Collections;
using Lazinator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Threading.Tasks;
using static LazinatorCodeGen.Roslyn.AdhocWorkspaceManager;
using LazinatorCodeGen.Roslyn;
using System.Collections.Immutable;
using Lazinator.Attributes;

namespace LazinatorTests.Tests
{
    public class RoslynPropertyParsing
    {
        [Fact]
        public async Task CanParseInterfaceProperties()
        {
            LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(Example));
            var exampleInterface = lazinatorFiles.LookupSymbol("IExample");
            var properties = exampleInterface.GetPropertySymbols();
            properties[0].Name.Should().Be("MyBool");
            properties[0].GetMethod.Name.Should().Be("get_MyBool");
            properties[0].SetMethod.Name.Should().Be("set_MyBool");
            properties[0].Type.Name.Should().Be("Boolean");
        }


        [Fact]
        public async Task CanParseInterfaceAttributes()
        {
            LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(Example));
            INamedTypeSymbol exampleInterface = lazinatorFiles.LookupSymbol("IExample");
            ImmutableArray<AttributeData> attributes = exampleInterface.GetAttributes();
            attributes.Count().Should().Be(1);
            Attribute converted = AttributeConverter.ConvertAttribute(attributes[0]);
            LazinatorAttribute lazinatorAttribute = converted as LazinatorAttribute;
            lazinatorAttribute.UniqueID.Should().NotBe(0);
        }

        [Fact]
        public async Task CanParseSupportedCollections()
        {
            LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(DotNetList_SelfSerialized));

            string interfaceName = "IDotNetList_SelfSerialized";
            var interfaceSymbol = lazinatorFiles.LookupSymbol(interfaceName);
            var properties = lazinatorFiles.PropertiesForType[interfaceSymbol];
            var property = properties.First().property;
            property.Type.Name.Should().Be("List");
            (property.Type is INamedTypeSymbol).Should().BeTrue();
            var namedType = property.Type as INamedTypeSymbol;
            namedType.TypeArguments.Count().Should().Be(1);
            var typeArgument = namedType.TypeArguments[0];
            typeArgument.Name.Should().Be("ExampleChild");
            INamedTypeSymbol exampleChildClass = lazinatorFiles.LookupSymbol(typeArgument.Name);
            exampleChildClass.IsReferenceType.Should().BeTrue();
        }

        [Fact]
        public async Task CanParseInterfacePropertiesWithInheritance()
        {
            LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(ExampleChildInherited));

            // load the inherited interface and make sure its properties and base properties can be parsed
            string interfaceName = nameof(IExampleChildInherited);
            var properties = lazinatorFiles.PropertiesForType[lazinatorFiles.LookupSymbol(interfaceName)];
            var propertiesThisLevel = properties.Where(x => x.isThisLevel).Select(x => x.property).ToList();
            var propertiesLowerLevels = properties.Where(x => !x.isThisLevel).Select(x => x.property).ToList();
            propertiesThisLevel.Count().Should().Be(1);
            propertiesLowerLevels.Count().Should().Be(2);
            propertiesThisLevel[0].Name.Should().Be("MyInt");
            propertiesThisLevel[0].GetMethod.Name.Should().Be("get_MyInt");
            propertiesThisLevel[0].SetMethod.Name.Should().Be("set_MyInt");
            propertiesThisLevel[0].Type.Name.Should().Be("Int32");
            propertiesLowerLevels[0].Name.Should().Be("MyLong");
            propertiesLowerLevels[0].GetMethod.Name.Should().Be("get_MyLong");
            propertiesLowerLevels[0].SetMethod.Name.Should().Be("set_MyLong");
            propertiesLowerLevels[0].Type.Name.Should().Be("Int64");
            propertiesLowerLevels[1].Name.Should().Be("MyShort");
            propertiesLowerLevels[1].GetMethod.Name.Should().Be("get_MyShort");
            propertiesLowerLevels[1].SetMethod.Name.Should().Be("set_MyShort");
            propertiesLowerLevels[1].Type.Name.Should().Be("Int16");
        }

        [Fact]
        public async Task CanParseIntermediateInterfacePropertiesWithInheritance()
        {
            LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(ExampleChild));

            // make sure we can also parse the intermediate type IExampleChild
            string intermediateInterfaceName = nameof(IExampleChild);
            var properties = lazinatorFiles.PropertiesForType[lazinatorFiles.LookupSymbol(intermediateInterfaceName)];
            var propertiesThisLevel = properties.Where(x => x.isThisLevel).Select(x => x.property).ToList();
            var propertiesLowerLevels = properties.Where(x => !x.isThisLevel).Select(x => x.property).ToList();
            propertiesThisLevel[0].Name.Should().Be("MyLong");
            propertiesThisLevel[0].GetMethod.Name.Should().Be("get_MyLong");
            propertiesThisLevel[0].SetMethod.Name.Should().Be("set_MyLong");
            propertiesThisLevel[0].Type.Name.Should().Be("Int64");
            propertiesThisLevel[1].Name.Should().Be("MyShort");
            propertiesThisLevel[1].GetMethod.Name.Should().Be("get_MyShort");
            propertiesThisLevel[1].SetMethod.Name.Should().Be("set_MyShort");
            propertiesThisLevel[1].Type.Name.Should().Be("Int16");
            propertiesLowerLevels.Count().Should().Be(0);
        }

        private static async Task<LazinatorCompilation> GetMiniRoslynFileSet(Type implementingType)
        {
            List<(string project, string mainFolder, string subfolder, string filename)> fileinfos = new List<(string project, string mainFolder, string subfolder, string filename)>()
            {
                ("Lazinator", "/Attributes/", "", "LazinatorAttribute"),
                ("LazinatorTests", "/Examples/", "Collections/", "DotNetList_SelfSerialized"),
                ("LazinatorTests", "/Examples/", "Collections/", "IDotNetList_SelfSerialized"),
                ("LazinatorTests", "/Examples/", "Hierarchy/", "Example"),
                ("LazinatorTests", "/Examples/", "Hierarchy/", "IExample"),
                ("LazinatorTests", "/Examples/", "Hierarchy/", "ExampleChild"),
                ("LazinatorTests", "/Examples/", "Hierarchy/", "IExampleChild"),
                ("LazinatorTests", "/Examples/", "Hierarchy/", "ExampleChildInherited"),
                ("LazinatorTests", "/Examples/", "Hierarchy/", "IExampleChildInherited"),
            };
            var ws = AdhocWorkspaceManager.CreateAdHocWorkspaceWithFiles(fileinfos);
            var compilation = await AdhocWorkspaceManager.GetCompilation(ws);
            var roslynFiles = new LazinatorCompilation(compilation, implementingType);
            return roslynFiles;
        }
    }
}
