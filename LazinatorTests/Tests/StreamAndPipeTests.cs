using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using System.Buffers;

namespace LazinatorTests.Tests
{
    public class StreamAndPipeTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void GetMemoryStreamWorks()
        {
            Example e = GetTypicalExample();
            MemoryStream s = e.GetMemoryStream();
            ConfirmStreamEqual(e, s);
        }

        private void ConfirmStreamEqual(Example e, MemoryStream s)
        {
            byte[] b = s.ToByteArray();
            Example e2 = new Example()
            {
            };
            e2.HierarchyStorage(b);
            ExampleEqual(e, e2).Should().BeTrue();
        }

        [Fact]
        public async Task GetPipeWorks()
        {
            Example e = GetTypicalExample();
            (Pipe p, int bytes) = e.GetPipe();
            MemoryStream stream = new MemoryStream();
            await stream.WriteAsync(p.Reader, (ulong)bytes);
            ConfirmStreamEqual(e, stream);
        }
    }
}
