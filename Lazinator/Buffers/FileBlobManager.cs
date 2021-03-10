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
            throw new NotImplementedException();
        }

        public ValueTask WriteAsync(string path, Memory<byte> bytes)
        {
            throw new NotImplementedException();
        }
        public void Append(string path, Memory<byte> bytes)
        {
            throw new NotImplementedException(); // DEBUG
        }

        public ValueTask AppendAsync(string path, Memory<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public long GetLength(string path)
        {
            throw new NotImplementedException();
        }
    }
}
