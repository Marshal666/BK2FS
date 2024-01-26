using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipFileSystem;

namespace BK2_mod_translate_tool
{
    public class FolderStructure
    {

        static FolderStructure instance;

        static object locker = new object();

        public static FolderStructure Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                lock (locker)
                {
                    if (instance == null)
                        instance = new FolderStructure();
                }
                return instance;
            }
        }

        private FolderStructure() { }

        

        public bool Create(string mod, string data, string master, ref string[] textFiles, ref VirtualFileSystem fileSystem)
        {
            VirtualFileSystem FileSystem;

            FolderSystem fs = null;
            PakLoader loader = null;
            PakLoader loader2 = null;

            if (Utils.IsValidPath(mod))
            {
                fs = new FolderSystem(mod);
                loader = new PakLoader(mod);
            }

            if (Utils.PathExists(data))
            {
                if (fs == null)
                {
                    fs = new FolderSystem(data);
                    loader = new PakLoader(data);
                }
                else
                {
                    fs.AddDir(data);
                    loader2 = new PakLoader(data);
                }
            }
            else if (fs == null)
                return false;

            FileSystem = new VirtualFileSystem(fs);
            fileSystem = FileSystem;

            if (loader != null)
                FileSystem.AddSystem(loader);
            if (loader2 != null)
                FileSystem.AddSystem(loader2);

            string[] files = FileSystem.GetAllFiles();

            textFiles = files.Where(path => Path.GetExtension(path).Equals(".txt", StringComparison.OrdinalIgnoreCase)).ToArray();

            return true;
        }

    }
}
