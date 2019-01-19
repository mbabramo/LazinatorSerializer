using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Xunit;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using LazinatorCodeGen.Roslyn;
using System.Collections.Immutable;
using System.IO;
using LazinatorTests.Support;

namespace LazinatorTests.Tests
{
    public class RoslynPropertyParsing
    {
        // The following is too slow, but can be uncommented.
        //[Fact]
        //public async Task CanParseInterfaceProperties()
        //{
        //    LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(Example));
        //    var exampleInterface = lazinatorFiles.LookupSymbol("IExample");
        //    var properties = exampleInterface.GetPropertySymbols();
        //    properties[0].Name.Should().Be("MyBool");
        //    properties[0].GetMethod.Name.Should().Be("get_MyBool");
        //    properties[0].SetMethod.Name.Should().Be("set_MyBool");
        //    properties[0].Type.Name.Should().Be("Boolean");
        //}

        [Fact]
        public async Task CanParseInterfaceAttributes()
        {
            try
            {
                LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(Example));
                INamedTypeSymbol exampleInterface = lazinatorFiles.LookupSymbol("IExample");
                ImmutableArray<AttributeData> attributes = exampleInterface.GetAttributes();
                attributes.Count().Should().Be(1);
                Attribute converted = AttributeConverter.ConvertAttribute(attributes[0]);
                LazinatorAnalyzer.AttributeClones.CloneLazinatorAttribute cloneLazinatorAttribute = converted as LazinatorAnalyzer.AttributeClones.CloneLazinatorAttribute;
                cloneLazinatorAttribute.UniqueID.Should().NotBe(0);
            }
            catch (IOException)
            {
                // occurs rarely if other process is looking up same file at same time, so let's ignore it
            }
        }

        [Fact]
        public async Task CanParseSupportedCollections()
        {
            LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(DotNetList_Lazinator));

            string interfaceName = "IDotNetList_Lazinator";
            var interfaceSymbol = lazinatorFiles.LookupSymbol(interfaceName);
            var properties = lazinatorFiles.PropertiesForType[LazinatorCompilation.TypeSymbolToString(interfaceSymbol)];
            var property = properties.First().Property;
            property.Type.Name.Should().Be("List");
            (property.Type is INamedTypeSymbol).Should().BeTrue();
            var namedType = property.Type as INamedTypeSymbol;
            namedType.TypeArguments.Count().Should().Be(1);
            var typeArgument = namedType.TypeArguments[0];
            typeArgument.Name.Should().Be("ExampleChild");
            INamedTypeSymbol exampleChildClass = lazinatorFiles.LookupSymbol(typeArgument.Name);
            exampleChildClass.IsReferenceType.Should().BeTrue();
        }

        // The following are commented out because they are slow.

        //[Fact]
        //public async Task CanParseInterfacePropertiesWithInheritance()
        //{
        //    LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(ExampleChildInherited));

        //    // load the inherited interface and make sure its properties and base properties can be parsed
        //    string interfaceName = nameof(IExampleChildInherited);
        //    var properties = lazinatorFiles.PropertiesForType[lazinatorFiles.LookupSymbol(interfaceName)];
        //    var propertiesThisLevel = properties.Where(x => x.LevelInfo == PropertyWithDefinitionInfo.Level.IsDefinedThisLevel).Select(x => x.Property).ToList();
        //    var propertiesLowerLevels = properties.Where(x => x.LevelInfo != PropertyWithDefinitionInfo.Level.IsDefinedThisLevel).Select(x => x.Property).ToList();
        //    propertiesThisLevel.Count().Should().Be(1);
        //    propertiesLowerLevels.Count().Should().Be(5);
        //    propertiesThisLevel[0].Name.Should().Be("MyInt");
        //    propertiesThisLevel[0].GetMethod.Name.Should().Be("get_MyInt");
        //    propertiesThisLevel[0].SetMethod.Name.Should().Be("set_MyInt");
        //    propertiesThisLevel[0].Type.Name.Should().Be("Int32");
        //    propertiesLowerLevels[0].Name.Should().Be("ByteSpan");
        //    propertiesLowerLevels[0].GetMethod.Name.Should().Be("get_ByteSpan");
        //    propertiesLowerLevels[0].SetMethod.Name.Should().Be("set_ByteSpan");
        //    propertiesLowerLevels[0].Type.Name.Should().Be("ReadOnlySpan");
        //    propertiesLowerLevels[1].Name.Should().Be("MyExampleGrandchild");
        //    propertiesLowerLevels[1].GetMethod.Name.Should().Be("get_MyExampleGrandchild");
        //    propertiesLowerLevels[1].SetMethod.Name.Should().Be("set_MyExampleGrandchild");
        //    propertiesLowerLevels[1].Type.Name.Should().Be("ExampleGrandchild");
        //    propertiesLowerLevels[2].Name.Should().Be("MyLong");
        //    propertiesLowerLevels[2].GetMethod.Name.Should().Be("get_MyLong");
        //    propertiesLowerLevels[2].SetMethod.Name.Should().Be("set_MyLong");
        //    propertiesLowerLevels[2].Type.Name.Should().Be("Int64");
        //    propertiesLowerLevels[3].Name.Should().Be("MyShort");
        //    propertiesLowerLevels[3].GetMethod.Name.Should().Be("get_MyShort");
        //    propertiesLowerLevels[3].SetMethod.Name.Should().Be("set_MyShort");
        //    propertiesLowerLevels[3].Type.Name.Should().Be("Int16");
        //    propertiesLowerLevels[4].Name.Should().Be("MyWrapperContainer");
        //    propertiesLowerLevels[4].GetMethod.Name.Should().Be("get_MyWrapperContainer");
        //    propertiesLowerLevels[4].SetMethod.Name.Should().Be("set_MyWrapperContainer");
        //    propertiesLowerLevels[4].Type.Name.Should().Be("WrapperContainer");
        //}

        //[Fact]
        //public async Task CanParseIntermediateInterfacePropertiesWithInheritance()
        //{
        //    LazinatorCompilation lazinatorFiles = await GetMiniRoslynFileSet(typeof(ExampleChild));

        //    // make sure we can also parse the intermediate type IExampleChild
        //    string intermediateInterfaceName = nameof(IExampleChild);
        //    var properties = lazinatorFiles.PropertiesForType[lazinatorFiles.LookupSymbol(intermediateInterfaceName)];
        //    var propertiesThisLevel = properties.Where(x => x.LevelInfo == PropertyWithDefinitionInfo.Level.IsDefinedThisLevel).Select(x => x.Property).ToList();
        //    var propertiesLowerLevels = properties.Where(x => x.LevelInfo != PropertyWithDefinitionInfo.Level.IsDefinedThisLevel).Select(x => x.Property).ToList();
        //    propertiesThisLevel[0].Name.Should().Be("ByteSpan");
        //    propertiesThisLevel[0].GetMethod.Name.Should().Be("get_ByteSpan");
        //    propertiesThisLevel[0].SetMethod.Name.Should().Be("set_ByteSpan");
        //    propertiesThisLevel[0].Type.Name.Should().Be("ReadOnlySpan");
        //    propertiesThisLevel[1].Name.Should().Be("MyExampleGrandchild");
        //    propertiesThisLevel[1].GetMethod.Name.Should().Be("get_MyExampleGrandchild");
        //    propertiesThisLevel[1].SetMethod.Name.Should().Be("set_MyExampleGrandchild");
        //    propertiesThisLevel[1].Type.Name.Should().Be("ExampleGrandchild");
        //    propertiesThisLevel[2].Name.Should().Be("MyLong");
        //    propertiesThisLevel[2].GetMethod.Name.Should().Be("get_MyLong");
        //    propertiesThisLevel[2].SetMethod.Name.Should().Be("set_MyLong");
        //    propertiesThisLevel[2].Type.Name.Should().Be("Int64");
        //    propertiesThisLevel[3].Name.Should().Be("MyShort");
        //    propertiesThisLevel[3].GetMethod.Name.Should().Be("get_MyShort");
        //    propertiesThisLevel[3].SetMethod.Name.Should().Be("set_MyShort");
        //    propertiesThisLevel[3].Type.Name.Should().Be("Int16");
        //    propertiesLowerLevels.Count().Should().Be(0);
        //}

        private static async Task<LazinatorCompilation> GetMiniRoslynFileSet(Type implementingType)
        {
            List<(string project, string mainFolder, string subfolder, string filename)> fileinfos = new List<(string project, string mainFolder, string subfolder, string filename)>()
            {
                ("Lazinator", "/Attributes/", "", "LazinatorAttribute"),
                ("LazinatorTests", "/Examples/", "Collections/", "DotNetList_Lazinator"),
                ("LazinatorTests", "/Examples/", "Collections/", "IDotNetList_Lazinator"),
                ("LazinatorTests", "/Examples/", "ExampleHierarchy/", "Example"),
                ("LazinatorTests", "/Examples/", "ExampleHierarchy/", "IExample"),
                ("LazinatorTests", "/Examples/", "ExampleHierarchy/", "ExampleChild"),
                ("LazinatorTests", "/Examples/", "ExampleHierarchy/", "IExampleChild"),
                ("LazinatorTests", "/Examples/", "ExampleHierarchy/", "ExampleChildInherited"),
                ("LazinatorTests", "/Examples/", "ExampleHierarchy/", "IExampleChildInherited"),
            };
            var ws = AdhocWorkspaceManager.CreateAdHocWorkspaceWithFiles(fileinfos, ".g.cs");
            var compilation = await AdhocWorkspaceManager.GetCompilation(ws);
            var roslynFiles = new LazinatorCompilation(compilation, implementingType, null);
            return roslynFiles;
        }
    }
}
