using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.IO.Compression;
using System.Drawing;
using SharpCompress;
using SharpCompress.Archives.Zip;
using System.Reflection;

namespace ZipFileSystem
{
    public class PakLoader : IVirtualFileSystem
    {

        public List<ZipArchive> Paks = new List<ZipArchive>();

        public ILogger Logs;

        public Dictionary<string, ZipArchiveEntry> ZipEntries = new Dictionary<string, ZipArchiveEntry>();

        public PakLoader(string startDir, ILogger logger = null)
        {
            Logs = logger;
            OpenDirectory(startDir);
        }

        public void OpenDirectory(string dir)
        {
            var files = Directory.GetFiles(dir, "*.pak");
            Array.Sort(files);
            Array.Reverse(files);
            foreach(var f in files)
            {
                try
                {
                    ZipArchive za = ZipArchive.Open(f);
                    Paks.Add(za);
                    foreach(var entry in za.Entries)
                    {
                        if(ZipEntries.ContainsKey(entry.Key.FullPath()))
                        {
                            if(entry.LastModifiedTime > ZipEntries[entry.Key.FullPath()].LastModifiedTime)
                            {
                                ZipEntries[entry.Key.FullPath()] = entry;
                            }
                        } else
                        {
                            ZipEntries[entry.Key.FullPath()] = entry;
                        }
                    }
                } catch(Exception e)
                {
                    Logs?.WriteLine(e.Message, Color.Red);
                }
            }
        }

        public bool ContainsFile(string path)
        {
            return ZipEntries.ContainsKey(path.FullPath());
        }

        public Stream OpenFile(string path)
        {
            var cpath = path.FullPath();
            if (ContainsFile(cpath))
                return ZipEntries[cpath].OpenEntryStream();
            //here it might be worthwhile to print sth like "file {path} not found!"
            return null;
            //old method is not needed! tests say so
            /*var oo = OpenFileOld(path);
            Logs.WriteLine($"Failed dict method with: {cpath} original: {path}, oldr: {(oo == null ? "fail" : "success")}");
            return oo;*/
        }

        public Stream OpenFileOld(string path)
        {
            ZipArchiveEntry en = null;
            foreach (var a in Paks)
            {
                var f = a.GetEntry(path);
                if (f != null)
                {
                    if (en != null)
                    {
                        if (en.LastModifiedTime < f.LastModifiedTime)
                            en = f;
                    }
                    else
                    {
                        en = f;
                    }
                }
            }
            if (en == null)
                return null;
            return en.OpenEntryStream();
        }

        public string ReadTextFile(string path) 
        { 
            using(var s = OpenFile(path))
            {
                using(var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public byte[] ReadFileBytes(string path) 
        {
            return OpenFile(path).ReadAllBytes();
        }

        public void WriteToFile(string path, byte[] bytes)
        {
            throw new NotImplementedException("Cannot write to Files in .pak archives!");
        }

        public void WriteToFile(string path, string text)
        {
            throw new NotImplementedException("Cannot write to Files in .pak archives!");
        }

        public DateTime FileLastModifyTime(string path)
        {
            return ((DateTime)ZipEntries[path.FullPath()].LastModifiedTime).ToUniversalTime();
        }

        public string GetFileSource(string path)
        {
            using (var s = OpenFile(path))
            {
                //hack the fucking stream using reflection to get the source .pak file
                var f = s.GetType().GetField("_baseStream", BindingFlags.NonPublic | BindingFlags.Instance);
                var _baseStream = f.GetValue(s);
                var fs = _baseStream.GetType().GetField("_stream", BindingFlags.NonPublic | BindingFlags.Instance);
                var _stream = fs.GetValue(_baseStream);
                var fss = _stream.GetType().GetProperty("Stream", BindingFlags.NonPublic | BindingFlags.Instance);
                FileStream Stream = (FileStream)fss.GetValue(_stream);
                var r = Stream.Name;
                return "pak: " + r;
            }
        }

        public string[] GetAllFiles()
        {
            return ZipEntries.Keys.ToArray();
        }
    }

    

}
