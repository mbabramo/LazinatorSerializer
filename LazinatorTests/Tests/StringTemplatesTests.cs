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
            string template = $"{beginning}{templatesProcessor.CommandBlockString("if", "include,1", " jumps")}";
            string result = templatesProcessor.Process(template, new Dictionary<string, string>() { { "include", "1" } });
            result.Should().Be(beginning + " jumps");
        }


    }
}
