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
using Lazinator.Exceptions;

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
        public async Task LazinatorCanBePipelined()
        {
            Example e = GetTypicalExample();
            (Pipe p, int bytes) = GetPipe(e);
            MemoryStream stream = new MemoryStream();
            await stream.WriteAsync(p.Reader, (ulong)bytes);
            ConfirmStreamEqual(e, stream);
        }


        /// <summary>
        /// Gets a pipe containing the Lazinator object. 
        /// </summary>
        /// <param name="lazinator"></param>
        /// <returns></returns>
        private static (Pipe pipe, int bytes) GetPipe(ILazinator lazinator)
        {
            lazinator.SerializeLazinator();
            Pipe pipe = new Pipe();
            AddToPipe(lazinator, pipe);
            pipe.Writer.Complete();
            if (lazinator.LazinatorMemoryStorage.Length > int.MaxValue)
                ThrowHelper.ThrowTooLargeException(int.MaxValue);
            return (pipe, (int)lazinator.LazinatorMemoryStorage.Length);
        }

        /// <summary>
        /// Writes a Lazinator object into a pipe.
        /// </summary>
        /// <param name="lazinator"></param>
        /// <param name="pipe"></param>
        private static void AddToPipe(ILazinator lazinator, Pipe pipe)
        {
            foreach (ReadOnlyMemory<byte> memoryBlock in lazinator.LazinatorMemoryStorage.EnumerateReadOnlyMemory())
                pipe.Writer.Write(memoryBlock.Span);
        }
    }
}
