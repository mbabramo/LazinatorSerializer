using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    public class FileBlobManager : IBlobManager
    {
        Dictionary<string, FileStream> OpenFileStreams = new Dictionary<string, FileStream>();

        public IBlobMemoryAllocator MemoryAllocator { get; set; } = new DefaultBlobMemoryAllocator();


        public ReadOnlyMemory<byte> Read(string path, long offset, int length)
        {
            using FileStream fs = File.OpenRead(path);
            var target = MemoryAllocator.Allocate(path, offset, length);
            if (offset != 0)
                fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(target.Span);
            return target;
        }

        public async ValueTask<ReadOnlyMemory<byte>> ReadAsync(string path, long offset, int length)
        {
            using FileStream fs = File.OpenRead(path);
            var target = MemoryAllocator.Allocate(path, offset, length);
            if (offset != 0)
                fs.Seek(offset, SeekOrigin.Begin);
            await fs.ReadAsync(target);
            return target;
        }

        public void Write(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            fs.SetLength(bytes.Span.Length);
            fs.Write(bytes.Span);
            MemoryAllocator.FreeMemory(path);
        }

        public async ValueTask WriteAsync(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            fs.SetLength(bytes.Span.Length);
            await fs.WriteAsync(bytes);
            MemoryAllocator.FreeMemory(path);
        }

        public void OpenForWriting(string path)
        {
            FileStream fs = File.OpenWrite(path);
            OpenFileStreams[path] = fs;
            MemoryAllocator.FreeMemory(path);
        }

        public void Append(string path, Memory<byte> bytes)
        {
            FileStream fs;
            bool fileStreamAlreadyOpen = false;
            if (OpenFileStreams.ContainsKey(path))
            {
                fileStreamAlreadyOpen = true;
                fs = OpenFileStreams[path];
            }
            else
            {
                fs = File.OpenWrite(path);
            }
            fs.Seek(0, SeekOrigin.End);
            long length = fs.Length;
            fs.Write(bytes.Span);
            if (!fileStreamAlreadyOpen)
                fs.SetLength(length + bytes.Length);

            if (!fileStreamAlreadyOpen)
            {
                fs.Close();
            }
        }

        public async ValueTask AppendAsync(string path, Memory<byte> bytes)
        {
            FileStream fs;
            bool fileStreamAlreadyOpen = false;
            if (OpenFileStreams.ContainsKey(path))
            {
                fileStreamAlreadyOpen = true;
                fs = OpenFileStreams[path];
            }
            else
            {
                fs = File.OpenWrite(path);
            }
            fs.Seek(0, SeekOrigin.End);
            long length = fs.Length;
            await fs.WriteAsync(bytes);
            if (!fileStreamAlreadyOpen)
                fs.SetLength(length + bytes.Length);

            if (!fileStreamAlreadyOpen)
            {
                fs.Close();
            }
        }

        public void CloseAfterWriting(string path)
        {
            var fs = OpenFileStreams[path];
            fs.SetLength(fs.Position);
            fs.Close();
            OpenFileStreams.Remove(path);
            MemoryAllocator.FreeMemory(path);
        }

        public long GetLength(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.Length;
        }

        public void Delete(string path)
        {
            File.Delete(path);
            MemoryAllocator.FreeMemory(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
