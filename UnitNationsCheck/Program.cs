using System;
using ZipFileSystem;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Security;

class Program
{

    public class TechYearsData
    {
        public HashSet<string> Sides = new ();

        public Dictionary<string, HashSet<string>> UnitNations = new ();

    }

    static void Main(string[] args)
    {
        ConsoleLogger cl = new ConsoleLogger();
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

        TechYearsData tyd = new ();

        var pkv = pk.ZipEntries.Values;
        var sl = new List<string>();
        foreach(var val in pkv)
        {
            string str = val.ToString().ToLower().Replace('\\', '/');
            if(str.StartsWith("units/technics/gb/tanks") && str.EndsWith("/") && !str.Contains(".mayaswatches") && !str.EndsWith("tanks/"))
            {
                sl.Add(str);
            }
        }

        foreach (var side in data.Sides.Items)
        {
            //fl.WriteLine("    " + $"{ix++}: " + side.NameFileRef.GetFileContents(FileSystem));
            tyd.Sides.Add(side.NameFileRef.GetFileContents(FileSystem));
        }

        nint ix = 0;
        foreach (var side in data.Sides.Items)
        {
            
            int inx = 0;
            foreach (var year in data.TechLevels.Items)
            {
                
                var level = side.TechLevels.Items[inx];
                
                var startingUnits = (Reinforcement)reinfSer.Deserialize(new MemoryStream(level.StartingUnits.GetFileContentsBin(FileSystem)));
                Dictionary<string, int> units = new Dictionary<string, int>();
                foreach (var unit in startingUnits.Entries.Items)
                {
                    if (!string.IsNullOrEmpty(unit.MechUnit.FormattedRef))
                    {
                        var ss = unit.MechUnit.GetFileContentsBin(FileSystem).ToMemoryStream();
                        if (ss == null)
                            continue;
                        MechUnitRPGStats stats = (MechUnitRPGStats)mechStatsSer.Deserialize(ss);
                        if (stats == null)
                            continue;
                        string name = unit.MechUnit.FormattedRef;
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
                    else if (!string.IsNullOrEmpty(unit.Squad.FormattedRef))
                    {
                        SquadRPGStats stats = (SquadRPGStats)squadSer.Deserialize(unit.Squad.GetFileContentsBin(FileSystem).ToMemoryStream());
                        string name = unit.Squad.FormattedRef;
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
                foreach (var reinfs in level.Reinforcements.Items)
                {
                    Reinforcement reinf = null;
                    try
                    {
                        reinf = (Reinforcement)reinfSer.Deserialize(reinfs.GetFileContents(FileSystem).GetBytes().ToMemoryStream());
                    }
                    catch (Exception ex)
                    {
                        //if (!ex.Message.Contains("XML document"))
                        //{
                        //    Console.WriteLine($"Error for file: {ex.Message} - year: {year.NameFileRef.GetFileContents(FileSystem)} - " +
                        //        $"nation: {side.NameFileRef.GetFileContents(FileSystem)} - " +
                        //        $"reinf file: {level.StartingUnits.FormattedRef}");
                        //}
                    }
                    if (reinf == null)
                        continue;
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
                                    continue;
                                }
                                string name = unit.MechUnit.FormattedRef;
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
                                    continue;
                                }
                                string name = unit.Squad.FormattedRef;
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
                        //sb.Append("                Empty: \n");
                    }
                    ix = 0;
                    string sidestr = side.NameFileRef.GetFileContents(FileSystem);
                    foreach (var unit in units)
                    {
                        if (tyd.UnitNations.ContainsKey(unit.Key))
                            tyd.UnitNations[unit.Key].Add(sidestr);
                        else
                        {
                            tyd.UnitNations.Add(unit.Key, new HashSet<string>());
                            tyd.UnitNations[unit.Key].Add(sidestr);
                        }
                    }
                }
                inx++;
            }
        }

        cl.WriteLine("tyd inited!");

        StringBuilder sb = new StringBuilder();
        foreach (var un in tyd.UnitNations)
        {
            sb.Append($"{un.Key} > {string.Join(" <> ", un.Value)}\n");
        }
        File.WriteAllText("labels.txt", sb.ToString());

        cl.WriteLine("Done!");
    }

}