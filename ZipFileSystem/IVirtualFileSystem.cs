using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipFileSystem
{
    public interface IVirtualFileSystem
    {

        DateTime FileLastModifyTime(string path);

        bool ContainsFile(string path);

        Stream OpenFile(string path);

        string ReadTextFile(string path);

        byte[] ReadFileBytes(string path);

        void WriteToFile(string path, byte[] bytes);

        void WriteToFile(string path, string text);

        string GetFileSource(string path);

        string[] GetAllFiles();

    }
}
