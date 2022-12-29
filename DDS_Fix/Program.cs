using System;
using System.Text;
using System.IO.Pipes;
using ZipFileSystem;

class Program
{

    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("No args given");
            return;
        }
        ConsoleLogger cl = new ConsoleLogger();
        FolderSystem fs = new FolderSystem(args[0]);
        VirtualFileSystem FileSystem = new VirtualFileSystem(fs);
        PakLoader pk;
        FileSystem.AddSystem(pk = new PakLoader(args[0], cl));
        string targetDir = "out";
        if (args.Length > 2)
        {
            targetDir = args[1];
        }
        string[] files = FileSystem.GetAllFiles();

        foreach (string file in files)
        {
            if (file.EndsWith(".dds") && file.ToLower().Contains("units") && !file.ToLower().Contains("icon"))
            {
                Console.WriteLine("Processing: " + file);
                byte[] data = FileSystem.ReadFileBytes(file);
                CreateFile(file, data);

            }
        }
        return;
    }

    public static void CreateFile(string filePath, byte[] data)
    {
        if (!File.Exists(filePath))
        {
            var parent = Directory.GetParent(filePath);
            Directory.CreateDirectory(parent.FullName);
            using (var f = File.Create(filePath)) { f.Write(data); }
        }
    }

}