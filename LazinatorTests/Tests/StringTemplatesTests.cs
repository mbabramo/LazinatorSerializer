using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using System.Threading.Tasks;
using System.Buffers;
using System;
using System.Security.Cryptography.X509Certificates;
using LazinatorAnalyzer.Support;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LazinatorTests.Tests
{
    public class StringTemplatesTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void TemplateWithoutCommandWorks()
        {
            string template = "The quick brown fox";
            StringTemplates templatesProcessor = new StringTemplates();
            string result = templatesProcessor.Process(template);
            result.Should().Be(template);
        }

        [Fact]
        public void TemplateIfCommandWorks_Including()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string beginning = "The quick brown fox";
            string template = $"{beginning}{templatesProcessor.CreateIfBlock("include", "1", " jumps")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "include", "1" } });
            result.Should().Be(beginning + " jumps");
        }

        [Fact]
        public void TemplateIfCommandWorks_Excluding()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string beginning = "The quick brown fox";
            string template = $"{beginning}{templatesProcessor.CreateIfBlock("include", "1", " jumps")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "include", "0" } });
            result.Should().Be(beginning);
        }

        [Fact]
        public void TemplateIfCommandWorks_Nested()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string beginning = "The quick brown fox";
            string middle = " jumps over";
            string end = " the lazy dog";
            string template = $"{beginning}{templatesProcessor.CreateIfBlock("outer", "1", $"{middle}{templatesProcessor.CreateIfBlock("inner", "1", end)}")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "outer", "0" }, { "inner", "0" } });
            result.Should().Be(beginning); 
            result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "outer", "0" }, { "inner", "1" } });
            result.Should().Be(beginning);
            result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "outer", "1" }, { "inner", "0" } });
            result.Should().Be(beginning + middle);
            result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "outer", "1" }, { "inner", "1" } });
            result.Should().Be(beginning + middle + end);
        }

        [Fact]
        public void TemplateForCommand_NotUsingVariable()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string template = $"{templatesProcessor.CreateForBlock("i", 0, 3, text)}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() {  });
            result.Should().Be($"{text}{text}{text}");
        }

        [Fact]
        public void TemplateForCommand_UsingVariable()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string template = $"{templatesProcessor.CreateForBlock("i", 0, 3, text + templatesProcessor.CreateVariableBlock("i"))}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}0{text}1{text}2");
        }

        [Fact]
        public void TemplateForCommand_IfVariable()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string template = $"{templatesProcessor.CreateForBlock("i", 0, 3, text + templatesProcessor.CreateIfBlock("i", "1", "HERE"))}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{text}HERE{text}");
        }

        [Fact]
        public void TemplateSetVariableCommand()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.CreateSetVariableBlock("i", "1")}{templatesProcessor.CreateIfBlock("i", "1", additionalText)}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{additionalText}");
        }


        [Fact]
        public void TemplateSetVariableCommand_EarlierIfCommandLeftUnevaluated()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.CreateIfBlock("i", "1", additionalText)}{templatesProcessor.CreateSetVariableBlock("i", "1")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().NotBe($"{text}{additionalText}");
            // the result will be {text} plus the unevaluated IF command.
        }

        [Fact]
        public void TemplateSetVariableCommand_LaterChangeInInitiallyNullVariableRegistered()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            // The initial If command will see that the variable i is null and it will not be processed initially. Then it will be processed within the 
            // broader reprocess block after the variable i has been set.
            string template = $"{text}{templatesProcessor.CreateIfBlock("i", "1", additionalText)}{templatesProcessor.CreateSetVariableBlock("i", "1")}";
            template = templatesProcessor.CreateReprocessBlock(template, 0);
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{additionalText}");
        }

        [Fact]
        public void TemplateSetVariableCommand_LaterChangeInVariableRegisteredByEncoding()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            // The variable is set to 0 before the If block and to 1 after the if block. We want the If block to reflect what happens afterward. So, We can use two reprocess blocks. The inner reprocess block has a cycle constraint, and the result is that its commands are encoded so that they won't be evaluated. The initial evaluation will reduce the cycle count and then unencode, so when we get to the outer reprocess block, the inner reprocess block will need to be executed.
            string template = $"{text}{templatesProcessor.CreateSetVariableBlock("i", "0")}{templatesProcessor.CreateReprocessBlock(templatesProcessor.CreateIfBlock("i", "1", additionalText), 1)}{templatesProcessor.CreateSetVariableBlock("i", "1")}";
            template = templatesProcessor.CreateReprocessBlock(template, 0); // this is the outer reprocess block -- by the time it runs, everything within it will have run, but the inner reprocess block will have merely been unencoded, so the if command will not have run
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{additionalText}");
        }

        [Fact]
        public void TemplateAsync_WithoutAwaitCalls_MayBeAsync()
        {
            AsyncStringTemplates t = new AsyncStringTemplates() { MayBeAsync = true };
            string template = $"{t.MaybeAsyncAndNot($@"public {t.MaybeAsyncReturnType("int")} MyMethod{t.MaybeAsyncWordAsync()}() => {t.MaybeAsyncReturnValue("3")};")}";
            string result = t.Process(template, new Dictionary<string, string>());
            string DEBUG = t.GetCommandTreeString(template);
            string expected = $@"public int MyMethod() => 3;
public ValueTask<int> MyMethodAsync() => ValueTask.FromResult(3);
";
            result.Should().Be(expected);
        }

        [Fact]
        public void TemplateAsync_WithoutAwaitCalls_MayNotBeAsync()
        {
            AsyncStringTemplates t = new AsyncStringTemplates() { MayBeAsync = false };
            string template = $"{t.MaybeAsyncAndNot($@"public {t.MaybeAsyncReturnType("int")} MyMethod() => {t.MaybeAsyncReturnValue("3")};")}";
            string result = t.Process(template, new Dictionary<string, string>());
            string expected = $@"public int MyMethod() => 3;";
            result.Should().Be(expected);
        }

        [Fact]
        public void TemplateAsync_WithAwaitCalls_MayBeAsync()
        {
            AsyncStringTemplates t = new AsyncStringTemplates() { MayBeAsync = true };
            string template = $@"{t.MaybeAsyncAndNot($@"public {t.MaybeAsyncReturnType("int")} MyMethod{t.MaybeAsyncWordAsync()}()
{{
    {t.MaybeAsyncWordAwait()}MyOtherMethod{t.MaybeAsyncWordAsync()}();
    return {t.MaybeAsyncReturnValue("3")};
}}")}";
            string result = t.Process(template, new Dictionary<string, string>());
            string expected = $@"public int MyMethod()
{{
    MyOtherMethod();
    return 3;
}}
async public ValueTask<int> MyMethodAsync()
{{
    await MyOtherMethodAsync();
    return 3;
}}
";
            result.Should().Be(expected);
        }

        [Fact]
        public void TemplateAsync_CanManuallyConstructMaybeAsyncBlock()
        {
            AsyncStringTemplates t = new AsyncStringTemplates() { MayBeAsync = true };
            string template1 = $@"{t.MaybeAsyncAndNot($@"public {t.MaybeAsyncReturnType("int")} MyMethod{t.MaybeAsyncWordAsync()}()
{{
    {t.MaybeAsyncWordAwait()}MyOtherMethod{t.MaybeAsyncWordAsync()}();
    return {t.MaybeAsyncReturnValue("3")};
}}")}";
            string template2 = $@"{t.MaybeAsyncAndNot_Begin}public {t.MaybeAsyncReturnType("int")} MyMethod{t.MaybeAsyncWordAsync()}()
{{
    {t.MaybeAsyncWordAwait()}MyOtherMethod{t.MaybeAsyncWordAsync()}();
    return {t.MaybeAsyncReturnValue("3")};
}}{t.MaybeAsyncAndNot_End}";
            template2.Should().Be(template1);
        }


    }
}
