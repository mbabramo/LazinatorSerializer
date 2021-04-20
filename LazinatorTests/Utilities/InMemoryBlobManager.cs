using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Exceptions;
using Lazinator.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorTests.Utilities
{
    public class InMemoryBlobManager : IBlobManager
    {
        public Dictionary<string, ReadOnlyMemory<byte>> Storage = new Dictionary<string, ReadOnlyMemory<byte>>();
        public IBlobMemoryAllocator MemoryAllocator { get; set; } = new DefaultBlobMemoryAllocator();

        public Task<ILazinator> Get<TKey>(ILazinator key) where TKey : ILazinator
        {
            string stringKey = key.ToString();
            if (Storage.ContainsKey(stringKey))
            {
                ReadOnlyMemory<byte> memory = Storage[stringKey];
                LazinatorMemory lazinatorMemory = new LazinatorMemory(memory);
                ILazinator result;
                result = DeserializationFactory.Instance.CreateFromBytesIncludingID(lazinatorMemory);
                return Task.FromResult(result);
            }
            return Task.FromResult(default(ILazinator));
        }

        public Task<TValue> Get<TKey, TValue>(ILazinator key) where TKey : ILazinator where TValue : ILazinator
        {
            string stringKey = key.ToString();
            if (Storage.ContainsKey(stringKey))
            {
                ReadOnlyMemory<byte> memory = Storage[stringKey];
                LazinatorMemory lazinatorMemory = new LazinatorMemory(memory);
                TValue result;
                try
                {
                    result = (TValue)DeserializationFactory.Instance.CreateFromBytesIncludingID(lazinatorMemory);
                }
                catch (InvalidCastException)
                {
                    throw new LazinatorDeserializationException($"Item stored at key {key} was not of expected type {typeof(TValue)}.");
                }
                return Task.FromResult(result);
            }
            return Task.FromResult(default(TValue));
        }

        public ReadOnlyMemory<byte> Read(string path, long offset, int length)
        {
            var storedMemory = Storage[path].Slice((int)offset, length);
            var copy = MemoryAllocator.Allocate(path, offset, length);
            storedMemory.CopyTo(copy);
            return storedMemory;
        }

        public ValueTask<ReadOnlyMemory<byte>> ReadAsync(string path, long offset, int length)
        {
            return ValueTask.FromResult(Read(path, offset, length));
        }

        public Task Set<TKey>(TKey key, ILazinator value) where TKey : ILazinator
        {
            string stringKey = key.ToString();
            ReadOnlyMemory<byte> memory = value.SerializeToArray();
            Storage[stringKey] = memory;
            return Task.CompletedTask;
        }

        public Task Set<TKey, TValue>(TKey key, TValue value) where TKey : ILazinator where TValue : ILazinator
        {
            string stringKey = key.ToString();
            ReadOnlyMemory<byte> memory = value.SerializeToArray();
            Storage[stringKey] = memory;
            return Task.CompletedTask;
        }

        public void Write(string path, ReadOnlyMemory<byte> bytes)
        {
            Storage[path] = bytes;
            MemoryAllocator.FreeMemory(path);
        }

        public ValueTask WriteAsync(string path, ReadOnlyMemory<byte> bytes)
        {
            Write(path, bytes);
            return ValueTask.CompletedTask;
        }


        public void Append(string path, ReadOnlyMemory<byte> bytes)
        {
            if (!Exists(path))
                Storage[path] = new byte[0];
            ReadOnlyMemory<byte> existingBytes = Storage[path];
            byte[] allBytes = new byte[existingBytes.Length + bytes.Length];
            existingBytes.CopyTo(allBytes);
            bytes.CopyTo(new Memory<byte>(allBytes).Slice(existingBytes.Length));
            Storage[path] = allBytes;
        }

        public ValueTask AppendAsync(string path, ReadOnlyMemory<byte> bytes)
        {
            Append(path, bytes);
            return ValueTask.CompletedTask;
        }

        public long GetLength(string path)
        {
            return Storage[path].Length;
        }

        public void OpenForWriting(string path)
        {
            MemoryAllocator.FreeMemory(path);
        }


        public bool Exists(string path)
        {
            return Storage.ContainsKey(path);
        }

        public void CloseAfterWriting(string path)
        {
            MemoryAllocator.FreeMemory(path);
        }

        public void Delete(string path)
        {
            Storage.Remove(path);
            MemoryAllocator.FreeMemory(path);
        }
    }
}
