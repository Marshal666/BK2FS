
using System;
using ZipFileSystem;

static class Program
{

    static string? RequestCommand()
    {
        Console.WriteLine("Enter Command: ");
        return Console.ReadLine();
    }

    static void Main(string[] args)
    {
        if(args.Length <= 0)
        {
            Console.WriteLine("No args given!");
            return;
        }
        ConsoleLogger cl = new ConsoleLogger();
        FolderSystem fs = new FolderSystem(args[0]);
        VirtualFileSystem FileSystem = new VirtualFileSystem(fs);
        PakLoader pk;
        Console.WriteLine("Dirs used:");
        FileSystem.AddSystem(pk = new PakLoader(args[0], cl));
        Console.WriteLine('\t' + args[0]);
        for(int i = 1; i < args.Length; i++)
        {
            fs.AddDir(args[i]);
            pk.OpenDirectory(args[i]);
            Console.WriteLine('\t' + args[i]);
        }
        string cmd;
        bool run = true;
        while(run && !string.IsNullOrEmpty(cmd = RequestCommand()))
        {
            string[] words = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if(words.Length == 0)
            {
                Console.WriteLine("Invalid command!");
                continue;
            }
            switch(words[0])
            {
                case "exit":
                    run = false;
                    break;
                case "show":
                case "s":
                    if (words.Length == 1)
                    {
                        Console.WriteLine("Needs path to file as argument!");
                        continue;
                    }
                    string data = FileSystem.ReadTextFile(words[1].FormattedPath());
                    if (data != null)
                        Console.WriteLine(data);
                    else
                        Console.WriteLine($"File at {words[1].FormattedPath()} does not exist!");
                    break;
                case "source":
                case "src":
                    if (words.Length == 1)
                    {
                        Console.WriteLine("Needs path to file as argument!");
                        continue;
                    }
                    string datasrc = FileSystem.GetFileSource(words[1].FormattedPath());
                    if (datasrc != null)
                        Console.WriteLine(datasrc);
                    else
                        Console.WriteLine($"File at {words[1].FormattedPath()} does not exist!");
                    break;
                case "export":
                case "e":
                    if (words.Length < 3)
                    {
                        Console.WriteLine("Needs path to file and output path as argument!");
                        continue;
                    }
                    var data2 = FileSystem.ReadFileBytes(words[1].FormattedPath());
                    if (data2 != null)
                        File.WriteAllBytes(words[2], data2);
                    else
                        Console.WriteLine($"File at {words[1].FormattedPath()} does not exist!");
                    break;
                default: Console.WriteLine("Invalid command!");
                    break;
            }
        }
    }

}