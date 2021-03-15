using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryChunkReferenceList : IMemoryChunkReferenceList
    {
        public void CombineToSameChunk(int memoryChunkID, bool separateReferences) => CombineToSameChunk(0, MemoryChunkReferences.Count(), memoryChunkID, separateReferences);

        /// <summary>
        /// Combine entries in the memory chunk reference list, in preparation for writing multiple entries to a single blob. 
        /// The entries can be renumbered to a new memory chunk ID. Meanwhile, the entries can be combined into a single 
        /// reference only (if we want these separate chunks to be read into memory all at once) or their independence
        /// can be maintained but sharing the same memory chunk ID, since they will be in the same blob file. 
        /// </summary>
        /// <param name="startingWithIndex"></param>
        /// <param name="numEntriesToCompact"></param>
        /// <param name="memoryChunkID"></param>
        /// <param name="singleReferenceOnly"></param>
        public void CombineToSameChunk(int startingWithIndex, int numEntriesToCompact, int memoryChunkID, bool separateReferences)
        {
            var revisedReferences = new List<MemoryChunkReference>();
            int initialNumReferences = MemoryChunkReferences.Count();
            for (int i = 0; i < initialNumReferences; i++)
            {
                if ((i <= startingWithIndex && separateReferences) || (i < startingWithIndex && !separateReferences) || i >= startingWithIndex + numEntriesToCompact)
                    revisedReferences.Add(MemoryChunkReferences[i]);
                else
                {
                    if (!separateReferences)
                    {
                        var original = revisedReferences[startingWithIndex];
                        revisedReferences[startingWithIndex] = new MemoryChunkReference(memoryChunkID, original.Offset, original.Length + MemoryChunkReferences[i].Length);
                    }
                    else
                    {
                        var original = MemoryChunkReferences[i];
                        var previous = revisedReferences[i - 1];
                        MemoryChunkReference toAdd = new MemoryChunkReference(memoryChunkID, previous.Offset + previous.Length, original.Length);
                        revisedReferences.Add(toAdd);
                    }
                }
            }
            MemoryChunkReferences = revisedReferences;
        }
    }
}
