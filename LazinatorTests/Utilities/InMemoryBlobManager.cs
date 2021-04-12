﻿using Lazinator.Buffers;
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
        public Dictionary<string, Memory<byte>> Storage = new Dictionary<string, Memory<byte>>();

        public Task<ILazinator> Get<TKey>(ILazinator key) where TKey : ILazinator
        {
            string stringKey = key.ToString();
            if (Storage.ContainsKey(stringKey))
            {
                Memory<byte> memory = Storage[stringKey];
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
                Memory<byte> memory = Storage[stringKey];
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

        public Memory<byte> Read(string path, long offset, int length)
        {
            Memory<byte> bytes = Storage[path].Slice((int)offset, length);
            return bytes;
        }

        public ValueTask<Memory<byte>> ReadAsync(string path, long offset, int length)
        {
            return ValueTask.FromResult(Read(path, offset, length));
        }

        public Task Set<TKey>(TKey key, ILazinator value) where TKey : ILazinator
        {
            string stringKey = key.ToString();
            Memory<byte> memory = value.SerializeToArray();
            Storage[stringKey] = memory;
            return Task.CompletedTask;
        }

        public Task Set<TKey, TValue>(TKey key, TValue value) where TKey : ILazinator where TValue : ILazinator
        {
            string stringKey = key.ToString();
            Memory<byte> memory = value.SerializeToArray();
            Storage[stringKey] = memory;
            return Task.CompletedTask;
        }

        public void Write(string path, Memory<byte> bytes)
        {
            Storage[path] = bytes;
        }

        public ValueTask WriteAsync(string path, Memory<byte> bytes)
        {
            Write(path, bytes);
            return ValueTask.CompletedTask;
        }


        public void Append(string path, Memory<byte> bytes)
        {
            if (!Exists(path))
                Storage[path] = new byte[0];
            Memory<byte> existingBytes = Storage[path];
            byte[] allBytes = new byte[existingBytes.Length + bytes.Length];
            existingBytes.CopyTo(allBytes);
            bytes.CopyTo(new Memory<byte>(allBytes).Slice(existingBytes.Length));
            Storage[path] = allBytes;
        }

        public ValueTask AppendAsync(string path, Memory<byte> bytes)
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
        }


        public bool Exists(string path)
        {
            return Storage.ContainsKey(path);
        }

        public void CloseAfterWriting(string path)
        {
        }

        public void Delete(string path)
        {
            Storage.Remove(path);
        }
    }
}