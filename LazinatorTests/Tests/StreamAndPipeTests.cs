using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using static LazinatorTests.Utilities.PipelinesExtensions;
using System.Buffers;
using System;

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
            Example e2 = new Example(b);
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
