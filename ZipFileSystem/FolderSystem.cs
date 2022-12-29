using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipFileSystem
{
    public class FolderSystem : IVirtualFileSystem
    {

        public string DefaultDir;

        public List<string> Dirs = new List<string>();

        public FolderSystem(string defaultDir)
        {
            DefaultDir = defaultDir.FullPath();
            Dirs.Add(DefaultDir);
        }

        public void AddDir(string d)
        {
            Dirs.Add(d.FullPath());
        }

        public bool ContainsFile(string path)
        {
            foreach (var d in Dirs)
            {
                string ss = Path.Combine(d, path).FullPath();
                if (File.Exists(ss))
                {
                    return true;
                }
            }
            return false;
        }

        public DateTime FileLastModifyTime(string path)
        {
            string f = null;
            FileInfo fc = null;
            foreach (var d in Dirs)
            {
                string ss = Path.Combine(d, path).FullPath();
                if (File.Exists(ss))
                {
                    if (f == null)
                    {
                        f = ss;
                        fc = new FileInfo(f);
                        continue;
                    }
                    FileInfo fi = new FileInfo(ss);
                    if (fi.LastWriteTime > fc.LastWriteTime)
                        (f, fc) = (ss, fi);
                }
            }
            return fc.LastWriteTimeUtc;
        }

        public string[] GetAllFiles()
        {
            List<string> ret = new List<string>();
            foreach (var d in Dirs)
            {
                var fs = Directory.GetFiles(d);
                foreach (var f in fs)
                    ret.Add(f.FormattedPath());
            }
            return ret.ToArray();
        }

        public string GetFileSource(string path)
        {
            return "folder: " + Path.GetFullPath(path);
        }

        public Stream OpenFile(string path)
        {
            string f = null;
            FileInfo fc = null;
            foreach(var d in Dirs)
            {
                string ss = Path.Combine(d, path).FullPath();
                if (File.Exists(ss))
                {
                    if(f == null)
                    {
                        f = ss;
                        fc = new FileInfo(f);
                        continue;
                    }
                    FileInfo fi = new FileInfo(ss);
                    if (fi.LastWriteTime > fc.LastWriteTime)
                        (f, fc) = (ss, fi);
                }
            }
            if (f == null)
                return null;
            return File.OpenRead(f);
        }

        public byte[] ReadFileBytes(string path)
        {
            using(var s = OpenFile(path))
            {
                if (s != null)
                    return s.ReadAllBytes();
                else
                    return null;
            }
        }

        public string ReadTextFile(string path)
        {
            using (var s = OpenFile(path))
            {
                using(var ss = new StreamReader(s))
                {
                    return ss.ReadToEnd();
                }
            }
        }

        public void WriteToFile(string path, byte[] bytes)
        {
            string p = Path.Combine(DefaultDir, path);
            File.WriteAllBytes(p, bytes);
        }

        public void WriteToFile(string path, string text)
        {
            string p = Path.Combine(DefaultDir, path);
            File.WriteAllText(p, text);
        }
    }
}
