using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class FileBlobReader : IBlobReader
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
    }
}
