﻿using Lazinator.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Utilities
{
    public class BothBlobStorage : IBlobManager
    {
        // designed to find differences between two types of blob storage.
        // before using this, must clear out the files in the file blob manager

        IBlobManager firstStorageType = new FileBlobManager();
        IBlobManager secondStorageType = new InMemoryBlobManager();

        public void Append(string path, Memory<byte> bytes)
        {
            Debug.WriteLine($"Appending to {path}: {String.Join(",", bytes.ToArray())}");
            firstStorageType.Append(path, bytes);
            secondStorageType.Append(path, bytes);
        }

        public async ValueTask AppendAsync(string path, Memory<byte> bytes)
        {
            Debug.WriteLine($"Appending to {path}: {String.Join(",", bytes.ToArray())}");
            await firstStorageType.AppendAsync(path, bytes);
            await secondStorageType.AppendAsync(path, bytes);
        }

        public void CloseAfterWriting(string path)
        {
            firstStorageType.CloseAfterWriting(path);
            secondStorageType.CloseAfterWriting(path);
        }

        public void Delete(string path)
        {
            Debug.WriteLine($"Deleting {path}");
            firstStorageType.Delete(path);
            secondStorageType.Delete(path);
        }

        public bool Exists(string path)
        {
            bool firstResult = firstStorageType.Exists(path);
            bool secondResult = secondStorageType.Exists(path);
            if (firstResult != secondResult)
                throw new Exception();
            return firstResult;
        }

        public long GetLength(string path)
        {
            long firstResult = firstStorageType.GetLength(path);
            long secondResult = secondStorageType.GetLength(path);
            if (firstResult != secondResult)
                throw new Exception();
            return firstResult;
        }

        public void OpenForWriting(string path)
        {
            firstStorageType.OpenForWriting(path);
            secondStorageType.OpenForWriting(path);
        }

        public Memory<byte> Read(string path, long offset, int length)
        {
            Memory<byte> firstResult = firstStorageType.Read(path, offset, length);
            Memory<byte> secondResult = secondStorageType.Read(path, offset, length);
            if (firstResult.ToArray().SequenceEqual(secondResult.ToArray()) == false)
                throw new Exception();
            return firstResult;
        }

        public async ValueTask<Memory<byte>> ReadAsync(string path, long offset, int length)
        {
            Memory<byte> firstResult = await firstStorageType.ReadAsync(path, offset, length);
            Memory<byte> secondResult = await secondStorageType.ReadAsync(path, offset, length);
            if (firstResult.ToArray().SequenceEqual(secondResult.ToArray()) == false)
                throw new Exception();
            return firstResult;
        }

        public void Write(string path, Memory<byte> bytes)
        {
            Debug.WriteLine($"Writing at {path}: {String.Join(",", bytes.ToArray())}");
            firstStorageType.Write(path, bytes);
            secondStorageType.Write(path, bytes);
        }

        public async ValueTask WriteAsync(string path, Memory<byte> bytes)
        {
            Debug.WriteLine($"Writing at {path}: {String.Join(",", bytes.ToArray())}");
            await firstStorageType.WriteAsync(path, bytes);
            await secondStorageType.WriteAsync(path, bytes);
        }
    }
}
