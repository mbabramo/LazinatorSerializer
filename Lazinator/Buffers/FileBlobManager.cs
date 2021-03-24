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
        Dictionary<string, FileStream> OpenFileStreams = new Dictionary<string, FileStream>();

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
            fs.SetLength(bytes.Span.Length);
            fs.Write(bytes.Span);
        }

        public async ValueTask WriteAsync(string path, Memory<byte> bytes)
        {
            using FileStream fs = File.OpenWrite(path);
            fs.SetLength(bytes.Span.Length);
            await fs.WriteAsync(bytes);
        }

        public void OpenForWriting(string path)
        {
            FileStream fs = File.OpenWrite(path);
            OpenFileStreams[path] = fs;
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
                fs.Seek(0, SeekOrigin.End);
            }
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
                fs.Seek(0, SeekOrigin.End);
            }
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
        }

        public long GetLength(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.Length;
        }
    }
}
