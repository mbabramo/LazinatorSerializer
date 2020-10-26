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
            string template = $"{beginning}{templatesProcessor.IfBlockString("include", "1", " jumps")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "include", "1" } });
            result.Should().Be(beginning + " jumps");
        }

        [Fact]
        public void TemplateIfCommandWorks_Excluding()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string beginning = "The quick brown fox";
            string template = $"{beginning}{templatesProcessor.IfBlockString("include", "1", " jumps")}";
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
            string template = $"{beginning}{templatesProcessor.IfBlockString("outer", "1", $"{middle}{templatesProcessor.IfBlockString("inner", "1", end)}")}";
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
            string template = $"{templatesProcessor.ForBlockString("i", 0, 3, text)}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() {  });
            result.Should().Be($"{text}{text}{text}");
        }

        [Fact]
        public void TemplateForCommand_UsingVariable()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string template = $"{templatesProcessor.ForBlockString("i", 0, 3, text + templatesProcessor.VariableString("i"))}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}0{text}1{text}2");
        }

        [Fact]
        public void TemplateForCommand_IfVariable()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string template = $"{templatesProcessor.ForBlockString("i", 0, 3, text + templatesProcessor.IfBlockString("i", "1", "HERE"))}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{text}HERE{text}");
        }

        [Fact]
        public void TemplateSetVariableCommand()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.SetVariableString("i", "1")}{templatesProcessor.IfBlockString("i", "1", additionalText)}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{additionalText}");
        }


        [Fact]
        public void TemplateSetVariableCommand_AfterSibling()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.IfBlockString("i", "1", additionalText)}{templatesProcessor.SetVariableString("i", "1")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().NotBe($"{text}{additionalText}");
        }

        [Fact]
        public void TemplateSetVariableCommand_AfterSiblingWithReprocessBlock()
        {
            StringTemplates templatesProcessor = new StringTemplates();
            string text = "The quick brown fox";
            string additionalText = " jumps";
            string template = $"{text}{templatesProcessor.IfBlockString("i", "1", additionalText)}{templatesProcessor.SetVariableString("i", "1")}";
            template = templatesProcessor.ReprocessBlockString(template);
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { });
            result.Should().Be($"{text}{additionalText}");
        }


    }
}
