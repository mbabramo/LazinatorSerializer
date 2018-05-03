using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using Lazinator.CodeDescription;
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
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Lazinator.Spans;
using LazinatorAnalyzer.Settings;
using LazinatorCodeGen.Roslyn;
using LazinatorCodeGen.Support;
using LazinatorTests.Examples.Generics;
using LazinatorTests.Support;
using Newtonsoft.Json;
using LazinatorListContainer = LazinatorTests.Examples.Collections.LazinatorListContainer;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.NonLazinator;

namespace LazinatorTests.Tests
{
    public class LazinatorCodeGenTest
    {
        [Fact]
        public void CodeStringBuilderWorks()
        {
            string source = @"
public class MyAltClass
{
public void MyAltMethod()
   {
        }
}
public class MyOtherClass
{
    public void MyAltMethod(int myAltN)
{
int x = myAltN + 1;
int y = x + 1;
}
}
";
            //string source = @"if (x == 2) {y=3; z= 2;}";
            string expected = @"
public class MyAltClass
{
    public void MyAltMethod()
    {
    }
}
public class MyOtherClass
{
    public void MyAltMethod(int myAltN)
    {
        int x = myAltN + 1;
        int y = x + 1;
    }
}
";
            var builder = new CodeStringBuilder();
            builder.Append(source);
            var result = builder.ToString();
            if (result != expected)
                IdentifyStringDifferencePoint(expected, result);
            result.Should().Be(expected);
        }

        private void IdentifyStringDifferencePoint(string s1, string s2)
        {
            for (int i = 0; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1[i] == s2[i])
                    Debug.Write(s1[i]);
                else
                {
                    Debug.WriteLine("PROBLEM AT " + i);
                    break;
                }
            }
        }

        [Fact]
        public void CanJsonDeserializeConfigFile()
        {
            string configString = @"
                {
                  ""InterchangeMappings"": {
                    ""MyNamespace.A"": ""AnotherNamespace.A_Interchange"",
                    ""MyNamespace.B"": ""AnotherNamespace.B_Interchange""
                  }
                }";
            LazinatorConfig config = new LazinatorConfig(configString);
            config.InterchangeMappings["MyNamespace.A"].Should().Be("AnotherNamespace.A_Interchange");
            config.InterchangeMappings["MyNamespace.B"].Should().Be("AnotherNamespace.B_Interchange");
        }

        [Fact]
        public void CanJsonDeserializeConfigFile2()
        {
            string configString = @"
                {
                  ""InterchangeMappings"": {
                    ""LazinatorTests.Examples.NonLazinatorInterchangeableClass"": ""LazinatorTests.Examples.NonLazinatorInterchangeClass""
                  }
                }";
            LazinatorConfig config = new LazinatorConfig(configString);
            config.InterchangeMappings["LazinatorTests.Examples.NonLazinatorInterchangeableClass"].Should().Be("LazinatorTests.Examples.NonLazinatorInterchangeClass");
        }

        [Fact]
        public async Task CodeGenerationProducesActualCode_LazinatorBuiltIns()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            await CompleteGenerateCode(typeof(LazinatorByteSpan), project: "Lazinator", mainFolder: "/Spans/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorBitArray), project: "Lazinator", mainFolder: "/Spans/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperAbstract<>), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableStruct<>), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperBool), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperSByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperChar), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperDecimal), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperFloat), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperDouble), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperShort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperUshort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperInt), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperUint), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperLong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperUlong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperTimeSpan), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperDateTime), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperGuid), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperString), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorOffsetList), project: "Lazinator", mainFolder: "/Collections/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableBool), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableSByte), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableChar), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableDecimal), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableFloat), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableDouble), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableShort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableUshort), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableInt), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableUint), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableLong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableUlong), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableTimeSpan), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableDateTime), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperNullableGuid), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
            await CompleteGenerateCode(typeof(LazinatorWrapperReadOnlySpanChar), project: "Lazinator", mainFolder: "/Wrappers/", subfolder: "", ws);
        }

        //[Fact]
        //public void CodeGenerationProducesActualCode_MainLazinatorClasses()
        //{
        //    // The commented out ones can't be auto-generated because they use T : struct
        //    // await CompleteGenerateCode(typeof(LazinatorFastReadList<>), subfolder: "", project: "Lazinator", mainFolder: "/Collections/"); 
        //    // await CompleteGenerateCode(typeof(LazinatorOffsetList), subfolder: "", project: "Lazinator", mainFolder: "/Collections/");
        //    // And this one can't be done because we override a lot of the default logic. We could redesign it so that's not necessary.
        //    // await CompleteGenerateCode(typeof(LazinatorList<>), subfolder: "", project: "Lazinator", mainFolder: "/Collections/");
        //}

        [Fact]
        public async Task CodeGenerationProducesActualCode_TestExamples()
        {
            AdhocWorkspace ws = GetAdhocWorkspace();
            await CompleteGenerateCode(typeof(Example), "LazinatorTests", "/Examples/", "Hierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleChild), "LazinatorTests", "/Examples/", "Hierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleChildInherited), "LazinatorTests", "/Examples/", "Hierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleNonexclusiveInterfaceImplementer), "LazinatorTests", "/Examples/", "Hierarchy/", ws);
            await CompleteGenerateCode(typeof(ExampleStruct), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainer), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(ExampleStructContainingStruct), "LazinatorTests", "/Examples/", "Structs/", ws);
            await CompleteGenerateCode(typeof(Abstract1), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Abstract2), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Concrete3), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Abstract4), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(Concrete5), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(AbstractGeneric1<>), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteGeneric2b), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(ConcreteGeneric2a), "LazinatorTests", "/Examples/", "Abstract/", ws);
            await CompleteGenerateCode(typeof(DerivedLazinatorList<>), "LazinatorTests", "/Examples/", "Generics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenerics), "LazinatorTests", "/Examples/", "Generics/", ws);
            await CompleteGenerateCode(typeof(OpenGenericStayingOpen<>), "LazinatorTests", "/Examples/", "Generics/", ws);
            await CompleteGenerateCode(typeof(OpenGenericStayingOpenContainer), "LazinatorTests", "/Examples/", "Generics/", ws);
            await CompleteGenerateCode(typeof(ClosedGenerics), "LazinatorTests", "/Examples/", "Generics/", ws);
            await CompleteGenerateCode(typeof(NonLazinatorContainer), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(NonLazinatorInterchangeClass), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(FromNonLazinatorBase), "LazinatorTests", "/Examples/", "NonLazinator/", ws);
            await CompleteGenerateCode(typeof(UnofficialInterfaceIncorporator), "LazinatorTests", "/Examples/", "UnofficialInterfaces/", ws);
            await CompleteGenerateCode(typeof(Dictionary_Values_SelfSerialized), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetHashSet_SelfSerialized), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_Nested_NonSelfSerializable), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_NonSelfSerializable), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_SelfSerialized), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetList_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetQueue_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(DotNetStack_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(SpanAndMemory), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(Array_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(ArrayMultidimensional_Values), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(LazinatorListContainer), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(LazinatorListContainerGeneric<>), "LazinatorTests", "/Examples/", "Collections/", ws);
            await CompleteGenerateCode(typeof(StructTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RegularTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(KeyValuePairTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(NestedTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
            await CompleteGenerateCode(typeof(RecordTuple), "LazinatorTests", "/Examples/", "Tuples/", ws);
        }

        private static List<string> GetDirectories()
        {
            return new List<string>()
            {
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Attributes",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Collections",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Spans",
                ReadCodeFile.GetCodeBasePath("Lazinator") + "/Wrappers",
                ReadCodeFile.GetCodeBasePath("LazinatorTests") + "/Examples",
            };
        }

        private static async Task CompleteGenerateCode(Type existingType, string project, string mainFolder, string subfolder, AdhocWorkspace ws)
        {
            if (existingType.IsInterface)
                throw new Exception("Can complete generate code only on class implementing interface, not interface itself.");
            string projectPath = ReadCodeFile.GetCodeBasePath(project);
            string name = ReadCodeFile.GetNameOfType(existingType);
            ReadCodeFile.GetCodeInFile(projectPath, mainFolder, subfolder, name, ".g.cs", out string codeBehindPath, out string codeBehind);
            ReadCodeFile.GetCodeInFile(projectPath, "/", "", "LazinatorConfig", ".json", out string configPath, out string configText);
            LazinatorConfig config = null;
            if (configText != null)
                config = JsonConvert.DeserializeObject<LazinatorConfig>(configText);

            var compilation = await AdhocWorkspaceManager.GetCompilation(ws);
            LazinatorCompilation lazinatorCompilation = new LazinatorCompilation(compilation, existingType, config);

            var d = new ObjectDescription(lazinatorCompilation.ImplementingTypeSymbol, lazinatorCompilation);
            var result = d.GetCodeBehind();
            bool match = codeBehind == result;

            bool automaticallyFix = true; // Set to true to automatically update all test classes on the local development machine to a new format. This will trivially result in the test passing. Before doing this, submit all changes. After doing this, if code compiles, run all tests. Then set this back to false. 
            if (automaticallyFix && !match)
                File.WriteAllText(codeBehindPath, result);
            else
                match.Should().BeTrue();
        }

        private AdhocWorkspace GetAdhocWorkspace()
        {
            List<string> directories = GetDirectories();
            return AdhocWorkspaceManager.CreateAdHocWorkspaceWithAllFilesInProjects(directories);
        }
    }
}