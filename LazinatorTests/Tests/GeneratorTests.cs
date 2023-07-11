using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace LazinatorTests.Tests
{
    [UsesVerify]
    public class GeneratorTests
    {
        [Fact]
        public Task Test()
        {
            return Task.CompletedTask;
        }

    }
}
