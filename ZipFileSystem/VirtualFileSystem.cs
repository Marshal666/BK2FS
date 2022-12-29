using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipFileSystem
{

    /// <summary>
    /// Rules:
    ///     1. No backslash '\\' in paths
    ///     2. No slash at the beggining of the path
    ///     3. Paths are case sensitive (not really...)
    /// </summary>
    public class VirtualFileSystem : IVirtualFileSystem
    {

        public Dictionary<string, byte[]> Cache = new Dictionary<string, byte[]>();
        public Dictionary<string, DateTime> CacheTime = new Dictionary<string, DateTime>();

        public List<IVirtualFileSystem> Systems = new List<IVirtualFileSystem>();

        IVirtualFileSystem DefaultSystem;

        public VirtualFileSystem(IVirtualFileSystem defaultSystem)
        {
            DefaultSystem = defaultSystem;
            Systems.Add(DefaultSystem);
        }

        public void AddSystem(IVirtualFileSystem s) => Systems.Add(s);

        public bool ContainsFile(string path)
        {
            if (Cache.ContainsKey(path))
                return true;
            foreach(var s in Systems)
            {
                if (s.ContainsFile(path))
                    return true;
            }
            return false;
        }

        public DateTime FileLastModifyTime(string path)
        {
            if (CacheTime.ContainsKey(path))
                return CacheTime[path];
            if (!ContainsFile(path))
                throw new FileNotFoundException(path);
            DateTime ret = default;
            foreach (var s in Systems)
            {
                if (s.ContainsFile(path))
                {
                    if(ret == default)
                    {
                        ret = s.FileLastModifyTime(path);
                        continue;
                    }
                    var t = s.FileLastModifyTime(path);
                    if (ret < t)
                        ret = t;
                }
            }
            if (ret != default)
                CacheTime[path] = ret;
            return ret;
        }

        public string[] GetAllFiles()
        {
            List<string> ret = new List<string>();
            foreach(var s in Systems)
            {
                ret.AddRange(s.GetAllFiles());
            }
            return ret.ToHashSet().ToArray();
        }

        public string GetFileSource(string path)
        {
            string ret = null;
            DateTime ct = default;
            foreach (var s in Systems)
            {
                if (s.ContainsFile(path))
                {
                    DateTime t = s.FileLastModifyTime(path);
                    if (ret == null)
                    {
                        ret = s.GetFileSource(path);
                        ct = t;
                        continue;
                    }
                    if (ct < t)
                    {
                        ret = s.GetFileSource(path);
                        ct = t;
                    }
                }
            }
            return ret;
        }

        public Stream OpenFile(string path)
        {
            if (Cache.ContainsKey(path)) {
                return new MemoryStream(Cache[path]);
            }
            Stream ret = null;
            DateTime ct = default;
            foreach(var s in Systems)
            {
                if(s.ContainsFile(path))
                {
                    DateTime t = s.FileLastModifyTime(path);
                    if(ret == null)
                    {
                        ret = s.OpenFile(path);
                        ct = t;
                        continue;
                    }
                    if(ct < t)
                    {
                        ret.Close();
                        ret = s.OpenFile(path);
                        ct = t;
                    }
                }
            }
            if(ret != null)
			{
                Cache[path] = (new StreamReader(ret)).ReadToEnd().GetBytes();
                ret = new MemoryStream(Cache[path]);
            }
            return ret;
        }

        public byte[] ReadFileBytes(string path)
        {
            byte[] ret = null;
            DateTime ct = default;
            foreach(var s in Systems)
			{
                var b = s.ReadFileBytes(path);
                DateTime t = default;
                if(b != null)
				{
                    t = s.FileLastModifyTime(path);
                    if(ret != null)
					{
                        if(ct < t)
						{
                            ret = b;
                            ct = t;
						}
					} else
					{
                        ret = b;
                        ct = t;
					}
				}
			}
            return ret;
            /*using(var s = OpenFile(path))
            {
                if (s == null)
                    return null;
                return s.ReadAllBytes();
            }*/
        }

        public string ReadTextFile(string path)
        {
            using (var s = OpenFile(path))
            {
                if (s == null)
                    return null;
                using(var ss = new StreamReader(s))
                {
                    return ss.ReadToEnd();
                }
            }
        }

        public void WriteToFile(string path, byte[] bytes)
        {
            DefaultSystem.WriteToFile(path, bytes);
        }

        public void WriteToFile(string path, string text)
        {
            DefaultSystem.WriteToFile(path, text);
        }

    }
}
