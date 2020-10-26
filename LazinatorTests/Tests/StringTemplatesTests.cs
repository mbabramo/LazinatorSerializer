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
        public void TemplateSetVariableCommand_AfterSibling()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.CreateIfBlock("i", "1", additionalText)}{templatesProcessor.CreateSetVariableBlock("i", "1")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().NotBe($"{text}{additionalText}");
        }

        [Fact]
        public void TemplateSetVariableCommand_AfterSiblingWithReprocessBlock()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.CreateIfBlock("i", "1", additionalText)}{templatesProcessor.CreateSetVariableBlock("i", "1")}";
            template = templatesProcessor.CreateReprocessBlock(template);
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{additionalText}");
        }

        [Fact]
        public void TemplateAsync_WithoutAwaitCalls_MayBeAsync()
        {
            AsyncStringTemplates t = new AsyncStringTemplates();
            bool mayBeAsync = true;
            string template = $"{t.NotAsyncAndMaybeAsync(mayBeAsync, $@"public {t.MaybeAsyncBlockReturnType(mayBeAsync, "int")} MyMethod{t.MaybeAsyncWordAsync(mayBeAsync)}() => {t.MaybeAsyncReturnValue(mayBeAsync, "3")};")}";
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
            AsyncStringTemplates t = new AsyncStringTemplates();
            bool mayBeAsync = false;
            string template = $"{t.NotAsyncAndMaybeAsync(mayBeAsync, $@"public {t.MaybeAsyncBlockReturnType(mayBeAsync, "int")} MyMethod() => {t.MaybeAsyncReturnValue(mayBeAsync, "3")};")}";
            string result = t.Process(template, new Dictionary<string, string>());
            string expected = $@"public int MyMethod() => 3;";
            result.Should().Be(expected);
        }

        [Fact]
        public void TemplateAsync_WithAwaitCalls_MayBeAsync()
        {
            AsyncStringTemplates t = new AsyncStringTemplates();
            bool mayBeAsync = true;
            string template = $@"{t.NotAsyncAndMaybeAsync(mayBeAsync, $@"public {t.MaybeAsyncBlockReturnType(mayBeAsync, "int")} MyMethod{t.MaybeAsyncWordAsync(mayBeAsync)}()
{{
    {t.MaybeAsyncWordAwait(mayBeAsync)}MyOtherMethod{t.MaybeAsyncWordAsync(mayBeAsync)}();
    return {t.MaybeAsyncReturnValue(mayBeAsync, "3")};
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


    }
}
