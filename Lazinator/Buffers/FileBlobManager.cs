using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class FileBlobManager : IBlobManager
    {
        public Memory<byte> Read(string path, long offset, int length)
        {
            using FileStream fs = File.OpenRead(path);
            byte[] target = new byte[length];
            if (offset != 0)
                fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(target);
            return target;
        }

        public async ValueTask<Memory<byte>> ReadAsync(string path, long offset, int length)
        {
            using FileStream fs = File.OpenRead(path);
            byte[] target = new byte[length];
            if (offset != 0)
                fs.Seek(offset, SeekOrigin.Begin);
            await fs.ReadAsync(target);
            return target;
        }

        public void Write(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            fs.Write(bytes.Span);
            fs.Flush();
        }

        public async ValueTask WriteAsync(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            await fs.WriteAsync(bytes);
            await fs.FlushAsync();
        }

        public void Append(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            fs.Seek(0, SeekOrigin.End);
            fs.Write(bytes.Span);
        }

        public async ValueTask AppendAsync(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            fs.Seek(0, SeekOrigin.End);
            await fs.WriteAsync(bytes);
        }

        public long GetLength(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.Length;
        }
    }
}
