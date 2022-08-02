using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using Lazinator.Collections.Tuples;
using Lazinator.Buffers;
using LazinatorTests.Examples.ExampleHierarchy;
using System.Threading.Tasks;
using System.Diagnostics;
using Lazinator.Collections.Tree;
using LazinatorTests.Utilities;
using System.Buffers;
using Lazinator.Persistence;

namespace LazinatorTests.Tests
{
    
    public class BlobTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteBlobWorks(bool useFile)
        {
            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string path = GetPathForIndexAndBlobs(useFile);
            if (path == null)
                return;
            if (blobManager.Exists(path))
                blobManager.Delete(path);

            var data = new byte[] { 1, 2, 3 };
            blobManager.Write(path, data);
            long length = blobManager.GetLength(path);
            var result = blobManager.Read(path, 0, (int) length);
            result.ToArray().SequenceEqual(data).Should().BeTrue();

            data = new byte[] { 4, 5, 6 };
            blobManager.Write(path, data);
            length = blobManager.GetLength(path);
            result = blobManager.Read(path, 0, (int)length);
            result.ToArray().SequenceEqual(data).Should().BeTrue();
            result = blobManager.ReadAll(path);
            result.ToArray().SequenceEqual(data).Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AppendBlobWorks(bool useFile)
        {
            IBlobManager blobManager = useFile ? new FileBlobManager() : new InMemoryBlobManager();
            string path = GetPathForIndexAndBlobs(useFile);
            if (path == null)
                return;
            if (blobManager.Exists(path))
                blobManager.Delete(path);

            blobManager.OpenForWriting(path);
            var data = new byte[] { 1, 2, 3, 4, 5, 6 };
            blobManager.Append(path, data.Take(3).ToArray());
            blobManager.Append(path, data.Skip(3).ToArray());
            blobManager.CloseAfterWriting(path);
            long length = blobManager.GetLength(path);
            var result = blobManager.Read(path, 0, (int)length);
            result.ToArray().SequenceEqual(data).Should().BeTrue();

            blobManager.Delete(path);
            data = new byte[] { 1, 2, 3, 4, 5, 6 };
            blobManager.Append(path, data.Take(3).ToArray());
            blobManager.Append(path, data.Skip(3).ToArray());
            length = blobManager.GetLength(path);
            result = blobManager.Read(path, 0, (int)length);
            result.ToArray().SequenceEqual(data).Should().BeTrue();

        }

        private static string GetPathForIndexAndBlobs(bool useFile)
        {
            string path = @"C:\Users\Admin\Desktop\testfolder";
            if (useFile && !System.IO.Directory.Exists(path))
                return null; // ignore this error
            string fullPath = path + @"\testblob.blb";
            return fullPath;
        }
    }
}
