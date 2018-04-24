//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CodeFixes;
//using Microsoft.CodeAnalysis.Diagnostics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using TestHelper;
//using LazinatorAnalyzer;

//namespace LazinatorAnalyzer.Test
//{
//    [TestClass]
//    public class UnitTest : CodeFixVerifier
//    {
//        //No diagnostics expected to show up
//        [TestMethod]
//        public void MustUseSelfSerializeAttribute_WithNoError()
//        {
//            string test = GetTypeContainingBackingField(true, true);
//            VerifyCSharpDiagnostic(test);
//            test = GetTypeContainingBackingField(true, false);
//            VerifyCSharpDiagnostic(test);
//        }


//        //No diagnostics expected to show up
//        [TestMethod]
//        public void MustUseSelfSerializeAttribute_WithError()
//        {
//            var expected = new DiagnosticResult
//            {
//                Id = LazinatorAnalyzer.Lazin001,
//                Message = "Add the [SelfSerialize] attribute to the class or struct.",
//                Severity = DiagnosticSeverity.Error,
//                Locations =
//                    new[] {
//                        new DiagnosticResultLocation("Test0.cs", 5, 10)
//                    }
//            };

//            string test = GetTypeContainingBackingField(false, true);
//            VerifyCSharpDiagnostic(test, expected);
//            string revised = GetTypeContainingBackingField(true, true);
//            VerifyCSharpFix(test, revised, null, true);

//            test = GetTypeContainingBackingField(false, false);
//            VerifyCSharpDiagnostic(test, expected);
//            revised = GetTypeContainingBackingField(true, false);
//            VerifyCSharpFix(test, revised, null, true);
//        }

        
//        [TestMethod]
//        public void PrimitiveFieldInSelfSerializeMustUseAttribute_WithNoError()
//        {
//            string test = GetTypeWithPossiblyWrongChildField(isClass: true, keyFieldIsPrimitiveType: true, keyFieldHasWrongAttribute: false, keyFieldHasNoAttribute: false);
//            VerifyCSharpDiagnostic(test);
//            test = GetTypeWithPossiblyWrongChildField(isClass: false, keyFieldIsPrimitiveType: true, keyFieldHasWrongAttribute: false, keyFieldHasNoAttribute: false);
//            VerifyCSharpDiagnostic(test);
//        }

//        [TestMethod]
//        public void PrimitiveFieldInSelfSerializeMustUseAttribute_WithError()
//        {
//            var expected = new DiagnosticResult
//            {
//                Id = LazinatorAnalyzer.Lazin001,
//                Message = "Add the [Backing] attribute to this field.",
//                Severity = DiagnosticSeverity.Error,
//                Locations =
//                    new[] {
//                        new DiagnosticResultLocation("Test0.cs", 5, 10)
//                    }
//            };

//            string test = GetTypeWithPossiblyWrongChildField(isClass: true, keyFieldIsPrimitiveType: true, keyFieldHasWrongAttribute: true, keyFieldHasNoAttribute: false);
//            //DEBUG
//            //VerifyCSharpDiagnostic(test, expected);
//            //string revised = GetTypeWithPossiblyWrongChildField(isClass: true, keyFieldIsPrimitiveType: true, keyFieldHasWrongAttribute: false, keyFieldHasNoAttribute: false);
//            //VerifyCSharpFix(test, revised, null, true);
            
//        }

//        private static string GetTypeContainingBackingField(bool containsSelfSerializeAttribute, bool isClass)
//        {
//            string selfSerializeAttribute = containsSelfSerializeAttribute ? "[SelfSerialize]" : "";
//            string triggeringAttribute = "Backing"; // could also be SelfSerializeChild, but not worth a separate test
//            string classOrStruct = isClass ? "class" : "struct";
//            string test;
//            // we put our class/struct into a namespace so that we can make sure that attribute will have appropriate whitespace. (We could also left-align the whole thing.)
//            if (containsSelfSerializeAttribute)
//                test = 
//$@"namespace MyNamespace
//{{
//    {selfSerializeAttribute}
//    public partial {classOrStruct} SelfSerializingExample
//    {{
//        [{triggeringAttribute}]
//        private uint _MyUint;
//    }}
//}}
//";
//            else
//                test = 
//$@"namespace MyNamespace
//{{
//    public partial {classOrStruct} SelfSerializingExample
//    {{
//        [{triggeringAttribute}]
//        private uint _MyUint;
//    }}
//}}
//";
//            return test;
//        }

//        private static string GetTypeWithPossiblyWrongChildField(bool isClass, bool keyFieldIsPrimitiveType, bool keyFieldHasWrongAttribute, bool keyFieldHasNoAttribute)
//        {
//            if (keyFieldHasWrongAttribute && keyFieldHasNoAttribute)
//                throw new NotSupportedException(); // mutually inconsistent potential problems
//            string classOrStruct = isClass ? "class" : "struct";
//            string keyField = keyFieldIsPrimitiveType ? "public uint _MyUint3;" : "private SelfSerializingExampleChild _MyChild;";
//            string correctAttribute = keyFieldIsPrimitiveType ? "[Backing]" : "[SelfSerializeChild]";
//            string wrongAttribute = !keyFieldIsPrimitiveType ? "[Backing]" : "[SelfSerializeChild]";
//            string attributeUsed = keyFieldHasNoAttribute ? "" : (keyFieldHasWrongAttribute ? wrongAttribute : correctAttribute);
//            if (attributeUsed != "")
//                attributeUsed =
//$@"
//        " + attributeUsed;
//            string test =
//$@"namespace MyNamespace
//{{
//    [SelfSerialize]
//    public partial {classOrStruct} SelfSerializingExampleChild
//    {{
//    }}

//    [SelfSerialize]
//    public partial {classOrStruct} SelfSerializingExample
//    {{
//        /* include a method and a property that don't need an attribute */
//        internal void MyMethod()
//        {{
//        }}
//        internal short X {{ get; set; }}

//        /* The following are OK, because they all have the needed attributes */
//        [Backing]
//        private long _MyLong;
//        [SelfSerializeChild]
//        private SelfSerializingExampleChild _AnotherChild;
//        [Backing]
//        [Another]
//        private uint _MyUint;
//        [Another]
//        [Backing]
//        private bool _MyBool;
//        [Another]
//        [Backing]
//        [YetAnother]
//        private string _MyString;
//        /* And now the key field... */{attributeUsed}
//        {keyField}
//    }}
//}}
//";
//            return test;
//        }

//        protected override CodeFixProvider GetCSharpCodeFixProvider()
//        {
//            return new LazinatorCodeFixProvider();
//        }

//        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
//        {
//            return new LazinatorAnalyzer();
//        }
//    }
//}
