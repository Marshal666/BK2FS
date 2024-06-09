using System;
using ZipFileSystem;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

class Program
{

    static void Main(string[] args)
    {
        if (args.Length < 0)
            return;
        ConsoleLogger cl = new ConsoleLogger();
        FileLogger fl = new FileLogger("output.txt");
        FolderSystem fs = new FolderSystem(args[0]);
        VirtualFileSystem FileSystem = new VirtualFileSystem(fs);
        PakLoader pk;
        FileSystem.AddSystem(pk = new PakLoader(args[0], cl));
        if (args.Length > 1)
        {
            fs.AddDir(args[1]);
            pk.OpenDirectory(args[1]);
        }

        var f = FileSystem.ReadFileBytes("Consts/Test/Test_MultiplayerConsts.xdb");
        
        XmlSerializer mpConstsSer = new XmlSerializer(typeof(MultiplayerConsts));
        XmlSerializer reinfSer = new XmlSerializer(typeof(Reinforcement));
        XmlSerializer mechStatsSer = new XmlSerializer(typeof(MechUnitRPGStats));
        XmlSerializer squadSer = new XmlSerializer(typeof(SquadRPGStats));

        MultiplayerConsts data = (MultiplayerConsts)mpConstsSer.Deserialize(new MemoryStream(f));
        fl.WriteLine("# YAML format");
        fl.WriteLine("Tech Levels:");
        nint ix = 0;
        foreach (var year in data.TechLevels.Items)
        {
            fl.WriteLine($"    {ix++} Name: " + year.NameFileRef.GetFileContents(FileSystem, "Consts/Test"));
        }

        fl.WriteLine("Sides:");
        ix = 0;
        foreach(var side in data.Sides.Items)
        {
            fl.WriteLine("    " + $"{ix++}: " + side.NameFileRef.GetFileContents(FileSystem, "Consts/Test"));
        }

        fl.WriteLine("Units per Side/Tech level:");
        foreach(var side in data.Sides.Items)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(side.NameFileRef.GetFileContents(FileSystem));
            sb.Append(":\n");
            int inx = 0;
            foreach (var year in data.TechLevels.Items)
            {
                sb.Append("    ");
                sb.Append(year.NameFileRef.GetFileContents(FileSystem));
                sb.Append(":\n");
                var level = side.TechLevels.Items[inx];
                sb.Append($"        Starting Units:\n");
                var startingUnits = (Reinforcement)reinfSer.Deserialize(new MemoryStream(level.StartingUnits.GetFileContentsBin(FileSystem)));
                Dictionary<string, int> units = new Dictionary<string, int>();
                foreach (var unit in startingUnits.Entries.Items)
                {
                    if(!string.IsNullOrEmpty(unit.MechUnit.FormattedRef))
                    {
                        var ss = unit.MechUnit.GetFileContentsBin(FileSystem).ToMemoryStream();
                        if (ss == null)
                            continue;
                        MechUnitRPGStats stats = (MechUnitRPGStats)mechStatsSer.Deserialize(ss);
                        if (stats == null)
                            continue;
                        string name = stats.GetUnitName(FileSystem, currentPath: unit.MechUnit.FormattedRef.GetPath(), logger: cl);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            if (units.Keys.Contains(name))
                                units[name]++;
                            else
                                units[name] = 1;
                            //sb.Append("            ");
                            //sb.Append(name);
                            //sb.Append('\n');
                        }
                    } else if(!string.IsNullOrEmpty(unit.Squad.FormattedRef))
                    {
                        SquadRPGStats stats = (SquadRPGStats)squadSer.Deserialize(unit.Squad.GetFileContentsBin(FileSystem).ToMemoryStream());
                        string name = stats.GetUnitName(FileSystem, currentPath: unit.Squad.FormattedRef.GetPath(), logger: cl);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            if (units.Keys.Contains(name))
                                units[name]++;
                            else
                                units[name] = 1;
                            //sb.Append("            ");
                            //sb.Append(name);
                            //sb.Append('\n');
                        }
                    }
                }
                ix = 0;
                foreach (var unit in units)
                {
                    sb.Append($"            {(char)('a' + ix++)} ");
                    sb.Append(unit.Value);
                    sb.Append("x: ");
                    sb.Append(unit.Key.Replace("\"","\\\""));
                    sb.Append('\n');
                }
                sb.Append("        Reinforcements:\n");
                foreach(var reinfs in level.Reinforcements.Items)
                {
                    Reinforcement reinf = null;
                    try
                    {
                        reinf = (Reinforcement)reinfSer.Deserialize(reinfs.GetFileContents(FileSystem).GetBytes().ToMemoryStream());
                    } catch (Exception ex)
                    {
                        //if (!ex.Message.Contains("XML document"))
                        //{
                        //    Console.WriteLine($"Error for file: {ex.Message} - year: {year.NameFileRef.GetFileContents(FileSystem)} - " +
                        //        $"nation: {side.NameFileRef.GetFileContents(FileSystem)} - " +
                        //        $"reinf file: {level.StartingUnits.FormattedRef}");
                        //}
                        fl.WriteLine($"Error in {reinfs.FormattedRef}: {ex.Message}");
                    }
                    if (reinf == null)
                        continue;
                    sb.Append("            ");
                    sb.Append(reinf.Type);
                    sb.Append(":\n");
                    units.Clear();
                    if (reinf.Entries.Items != null)
                    {
                        foreach (var unit in reinf.Entries.Items)
                        {
                            if (!string.IsNullOrEmpty(unit.MechUnit.FormattedRef))
                            {
                                MechUnitRPGStats stats = null;
                                try
                                {
                                    var ss = unit.MechUnit.GetFileContentsBin(FileSystem).ToMemoryStream();
                                    if (ss == null)
                                        continue;
                                    stats = (MechUnitRPGStats)mechStatsSer.Deserialize(ss);
                                    if (stats == null)
                                        continue;
                                }
                                catch (Exception ex)
                                {
                                    //if(!ex.Message.Contains("XML document"))
                                    //{
                                    //    Console.WriteLine($"Error for file: {ex.Message} - year: {year.NameFileRef.GetFileContents(FileSystem)} - " +
                                    //        $"nation: {side.NameFileRef.GetFileContents(FileSystem)} - " +
                                    //        $"reinf file: {reinfs.FormattedRef}");
                                    //}
                                    fl.WriteLine($"Error reading {unit.MechUnit.FormattedRef}: " + ex.Message);
                                    continue;
                                }
                                string name = stats.GetUnitName(FileSystem, currentPath: unit.MechUnit.FormattedRef.GetPath(), logger: cl);
                                if (!string.IsNullOrWhiteSpace(name))
                                {
                                    if (units.Keys.Contains(name))
                                        units[name]++;
                                    else
                                        units[name] = 1;
                                    //sb.Append("                ");
                                    //sb.Append(name);
                                    //sb.Append('\n');
                                }
                            }
                            else if (!string.IsNullOrEmpty(unit.Squad.FormattedRef))
                            {
                                SquadRPGStats stats = null;
                                try
                                {
                                    stats = (SquadRPGStats)squadSer.Deserialize(unit.Squad.GetFileContentsBin(FileSystem).ToMemoryStream());
                                }
                                catch (Exception ex)
                                {
                                    fl.WriteLine($"Error reading {unit.Squad.FormattedRef}: " + ex.Message);
                                    continue;
                                }
                                string name = stats.GetUnitName(FileSystem, currentPath: unit.Squad.FormattedRef.GetPath(), logger: cl);
                                if (!string.IsNullOrWhiteSpace(name))
                                {
                                    if (units.Keys.Contains(name))
                                        units[name]++;
                                    else
                                        units[name] = 1;
                                    //sb.Append("                ");
                                    //sb.Append(name);
                                    //sb.Append('\n');
                                }
                            }
                        }
                    }
                    if (units.Count <= 0)
                    {
                        sb.Append("                Empty: \n");
                    }
                    ix = 0;
                    foreach (var unit in units)
                    {
                        sb.Append($"                {(char)('a' + ix++)} ");
                        sb.Append(unit.Value);
                        sb.Append("x: ");
                        sb.Append(unit.Key);
                        sb.Append('\n');
                    }
                }
                inx++;
            }
            fl.WriteLine(sb.ToString());
        }
        fl.Close();
    }

}