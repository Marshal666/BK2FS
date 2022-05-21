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

namespace ZipFileSystem
{
    public class PakLoader : IVirtualFileSystem
    {

        public List<ZipArchive> Paks = new List<ZipArchive>();

        public ILogger Logs;

        public PakLoader()
        {

        }

        public PakLoader(ILogger logger)
        {
            Logs = logger;
        }

        public PakLoader(string startDir, ILogger logger = null)
        {
            Logs = logger;
            OpenDirectory(startDir);
        }

        public void OpenDirectory(string dir)
        {
            foreach(var f in Directory.GetFiles(dir, "*.pak"))
            {
                try
                {
                    Paks.Add(ZipArchive.Open(f));
                } catch(Exception e)
                {
                    Logs?.WriteLine(e.Message, Color.Red);
                }
            }
        }

        public bool ContainsFile(string path)
        {
            foreach (var a in Paks)
            {
                var f = a.GetEntry(path);
                if (f != null)
                {
                    return true;
                }
            }
            return false;
        }

        public Stream OpenFile(string path)
        {
            ZipArchiveEntry en = null;
            foreach(var a in Paks)
            {
                var f = a.GetEntry(path);
                if(f != null)
                {
                    if(en != null)
                    {
                        if (en.LastModifiedTime < f.LastModifiedTime)
                            en = f;
                    } else
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
            return en.OpenEntryStream().ReadAllBytes();
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
            return ((DateTime)en.LastModifiedTime).ToUniversalTime();
        }
    }

    

}
