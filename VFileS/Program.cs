using System;
using ZipFileSystem;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

class Program
{

    static void Main(string[] args)
    {
        ConsoleLogger cl = new ConsoleLogger();
        FolderSystem fs = new FolderSystem(args[0]);
        VirtualFileSystem FileSystem = new VirtualFileSystem(fs);
        FileSystem.AddSystem(new PakLoader(args[0], cl));

        var f = FileSystem.ReadFileBytes("Consts/Test/Test_MultiplayerConsts.xdb");
        
        XmlSerializer mpConstsSer = new XmlSerializer(typeof(MultiplayerConsts));
        XmlSerializer reinfSer = new XmlSerializer(typeof(Reinforcement));
        XmlSerializer mechStatsSer = new XmlSerializer(typeof(MechUnitRPGStats));
        XmlSerializer squadSer = new XmlSerializer(typeof(SquadRPGStats));

        MultiplayerConsts data = (MultiplayerConsts)mpConstsSer.Deserialize(new MemoryStream(f));

        Console.WriteLine("Tech Levels:");
        foreach(var year in data.TechLevels.Items)
        {
            Console.WriteLine("Name: " + year.NameFileRef.GetFileContents(FileSystem));
        }

        Console.WriteLine("Sides:");
        foreach(var side in data.Sides.Items)
        {
            Console.WriteLine(side.NameFileRef.GetFileContents(FileSystem));
        }

        Console.WriteLine("Units per Side/Tech level:");
        foreach(var side in data.Sides.Items)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(side.NameFileRef.GetFileContents(FileSystem));
            sb.Append(":\n");
            int inx = 0;
            foreach (var year in data.TechLevels.Items)
            {
                sb.Append('\t');
                sb.Append(year.NameFileRef.GetFileContents(FileSystem));
                sb.Append(":\n");
                var level = side.TechLevels.Items[inx];
                sb.Append($"\t\tStarting Units:\n");
                var startingUnits = (Reinforcement)reinfSer.Deserialize(new MemoryStream(level.StartingUnits.GetFileContentsBin(FileSystem)));
                foreach (var unit in startingUnits.Entries.Items)
                {
                    if(!string.IsNullOrEmpty(unit.MechUnit.FormattedRef))
                    {
                        MechUnitRPGStats stats = (MechUnitRPGStats)mechStatsSer.Deserialize(unit.MechUnit.GetFileContentsBin(FileSystem).ToMemoryStream());
                        string name = stats.GetUnitName(FileSystem, currentPath: unit.MechUnit.FormattedRef.GetPath(), logger: cl);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            sb.Append("\t\t\t");
                            sb.Append(name);
                            sb.Append('\n');
                        }
                    } else if(!string.IsNullOrEmpty(unit.Squad.FormattedRef))
                    {
                        SquadRPGStats stats = (SquadRPGStats)squadSer.Deserialize(unit.Squad.GetFileContentsBin(FileSystem).ToMemoryStream());
                        string name = stats.GetUnitName(FileSystem, currentPath: unit.Squad.FormattedRef.GetPath(), logger: cl);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            sb.Append("\t\t\t");
                            sb.Append(name);
                            sb.Append('\n');
                        }
                    }
                }
                foreach(var reinfs in level.Reinforcements.Items)
                {
                    
                }
                inx++;
            }
            Console.WriteLine(sb.ToString());
        }
    }

}