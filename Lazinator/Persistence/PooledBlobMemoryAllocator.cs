using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Persistence
{
    public class PooledBlobMemoryAllocator : IBlobMemoryAllocator
    {
        private Dictionary<string, HashSet<long>> LoadedOffsets = new Dictionary<string, HashSet<long>>();
        private Dictionary<(string path, long offset), byte[]> StoredMemory = new Dictionary<(string path, long offset), byte[]>();

        public ArrayPool<byte> Pool { get; set; } = ArrayPool<byte>.Shared;

        public Memory<byte> Allocate(string forPath, long offset, int length)
        {
            if (!LoadedOffsets.ContainsKey(forPath))
                LoadedOffsets[forPath] = new HashSet<long>();
            HashSet<long> loadedOffsetsForPath = LoadedOffsets[forPath];
            if (loadedOffsetsForPath.Contains(offset))
                return StoredMemory[(forPath, offset)];
            loadedOffsetsForPath.Add(offset);
            byte[] allocatedArray = Pool.Rent(length);
            Memory<byte> allocatedMemory = new Memory<byte>(allocatedArray);
            StoredMemory[(forPath, offset)] = allocatedArray;
            if (allocatedArray.Length == length)
                return allocatedMemory;
            else
                return allocatedMemory.Slice(0, length);
        }

        public void FreeMemory(string forPath)
        {
            if (LoadedOffsets.ContainsKey(forPath))
            {
                HashSet<long> loadedOffsetsForPath = LoadedOffsets[forPath];
                foreach (long offset in loadedOffsetsForPath)
                {
                    byte[] allocatedArray = StoredMemory[(forPath, offset)];
                    StoredMemory.Remove((forPath, offset));
                    Pool.Return(allocatedArray);
                }
                LoadedOffsets.Remove(forPath);
            }
        }
    }
}
