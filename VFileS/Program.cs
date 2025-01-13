using System;
using ZipFileSystem;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using VFileS.XmlClasses;

class Program
{

    public static SquadRPGStats? LoadSquadRPGStats(
            XmlSerializer ser, 
            VirtualFileSystem FileSystem, 
            Reinforcement.EntriesClass.ItemClass unit
        )
    {
        return (SquadRPGStats?)ser.Deserialize(unit.Squad.GetFileContentsBin(FileSystem).ToMemoryStream());
    }

    public static SquadRPGStats? LoadSquadRPGStats(
        XmlSerializer ser,
        VirtualFileSystem FileSystem,
        FileRef unit
    )
    {
        return (SquadRPGStats?)ser.Deserialize(unit.GetFileContentsBin(FileSystem).ToMemoryStream());
    }

    public static MechUnitRPGStats? LoadMechUnitRPGStats(
            XmlSerializer ser, 
            VirtualFileSystem FileSystem, 
            Reinforcement.EntriesClass.ItemClass unit
        )
    {
        var ss = unit.MechUnit.GetFileContentsBin(FileSystem).ToMemoryStream();
        if (ss == null)
            return null;
        return (MechUnitRPGStats?)ser.Deserialize(ss);
    }

    public static MechUnitRPGStats? LoadMechUnitRPGStats(
        XmlSerializer ser,
        VirtualFileSystem FileSystem,
        FileRef unit
    )
    {
        var ss = unit.GetFileContentsBin(FileSystem).ToMemoryStream();
        if (ss == null)
            return null;
        return (MechUnitRPGStats?)ser.Deserialize(ss);
    }

    public static PartyDependentInfo? GetPartyInfo(MultiplayerConsts.SidesClass.ItemClass1 item, VirtualFileSystem fs)
    {
        XmlSerializer partySer = new XmlSerializer(typeof(PartyDependentInfo));
        return (PartyDependentInfo?)partySer.Deserialize(item.PartyInfo.GetFileContentsBin(fs).ToMemoryStream());
    }

    public static HashSet<string> ArtyDB_Types = new HashSet<string>() 
    {
        "DB_RPG_TYPE_ART_HEAVY_MG",
        "DB_RPG_TYPE_ART_MORTAR",
        "DB_RPG_TYPE_ART_GUN",
        "DB_RPG_TYPE_ART_AAGUN",
        "DB_RPG_TYPE_ART_HEAVY_GUN",
        "DB_RPG_TYPE_ART_HOWITZER",
        "DB_RPG_TYPE_ART_ROCKET",
        "DB_RPG_TYPE_ART_SUPER",
    };

    static void Main(string[] args)
    {
        if (args.Length < 0)
            return;
        ConsoleLogger cl = new ConsoleLogger();
        FileLogger fl = new FileLogger("output.yml");
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
            fl.WriteLine($"    {ix++} Name: " + year.NameFileRef.GetFileContents(FileSystem, "Consts/Test").Trim());
        }

        fl.WriteLine("Sides:");
        ix = 0;
        foreach(var side in data.Sides.Items)
        {
            fl.WriteLine("    " + $"{ix++}: " + side.NameFileRef.GetFileContents(FileSystem, "Consts/Test").Trim());
        }

        fl.WriteLine("Units per Side/Tech level:");
        foreach(var side in data.Sides.Items)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(side.NameFileRef.GetFileContents(FileSystem).Trim());
            PartyDependentInfo info = GetPartyInfo(side, FileSystem);

            // Game does this internally or something like that
            if(info.AAGunSquad == null)
            {
                info.AAGunSquad = info.GunCrewSquad;
            }

            if(
                    info.GunCrewSquad == null || 
                    info.HowitzerGunCrewSquad == null || 
                    info.ResupplyEngineerSquad == null || 
                    info.HeavyMachinegunSquad == null || 
                    info.AAGunSquad == null
                )
            {
                cl.WriteLine("Some important squad in PartyDependentInfo is null!!!\nShit might crash now...");
            }
            SquadRPGStats GunCrewSquad = LoadSquadRPGStats(squadSer, FileSystem, info.GunCrewSquad);
            SquadRPGStats HowitzerGunCrewSquad = LoadSquadRPGStats(squadSer, FileSystem, info.HowitzerGunCrewSquad);
            SquadRPGStats HeavyMachinegunSquad = LoadSquadRPGStats(squadSer, FileSystem, info.HeavyMachinegunSquad);
            SquadRPGStats AAGunSquad = LoadSquadRPGStats(squadSer, FileSystem, info.AAGunSquad);
            SquadRPGStats ResupplyEngineerSquad = LoadSquadRPGStats(squadSer, FileSystem, info.ResupplyEngineerSquad);

            Dictionary<string, SquadRPGStats> ArtyDB_Types = new()
            {
                { "DB_RPG_TYPE_ART_HEAVY_MG", HeavyMachinegunSquad },
                { "DB_RPG_TYPE_ART_MORTAR", GunCrewSquad },
                { "DB_RPG_TYPE_ART_GUN", GunCrewSquad },
                { "DB_RPG_TYPE_ART_AAGUN", AAGunSquad },
                { "DB_RPG_TYPE_ART_HEAVY_GUN", GunCrewSquad },
                { "DB_RPG_TYPE_ART_HOWITZER", GunCrewSquad },
                { "DB_RPG_TYPE_ART_ROCKET", GunCrewSquad },
                { "DB_RPG_TYPE_ART_SUPER", GunCrewSquad },
            };

            Dictionary<SquadRPGStats, FileRef> SquadPaths = new()
            {
                { GunCrewSquad, info.GunCrewSquad},
                { HowitzerGunCrewSquad, info.HowitzerGunCrewSquad},
                { HeavyMachinegunSquad, info.HeavyMachinegunSquad},
                { AAGunSquad, info.AAGunSquad},
                { ResupplyEngineerSquad, info.ResupplyEngineerSquad},
            };

            sb.Append(":\n");
            int inx = 0;
            foreach (var year in data.TechLevels.Items)
            {
                sb.Append("    ");
                sb.Append(year.NameFileRef.GetFileContents(FileSystem).Trim());
                sb.Append(":\n");
                var level = side.TechLevels.Items[inx];
                sb.Append($"        Starting Units:\n");
                var startingUnits = (Reinforcement)reinfSer.Deserialize(new MemoryStream(level.StartingUnits.GetFileContentsBin(FileSystem)));
                Dictionary<string, int> units = new Dictionary<string, int>();
                Dictionary<string, float> unitExps = new Dictionary<string, float>();
                float expSum = 0f;
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
                        if (name == null)
                            name = $"NAME_ERROR: {unit.MechUnit.FormattedRef}";
                        name = name.Trim();
                        expSum += stats.ExpPrice;
                        unitExps[name] = stats.ExpPrice;
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
                        if (ArtyDB_Types.Keys.Contains(stats.DBtype) && stats.HasGunners)
                        {
                            var squad = ArtyDB_Types[stats.DBtype];
                            name = squad.GetUnitName(FileSystem, SquadPaths[squad].FormattedRef.GetPath(), logger: cl);

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                name = $"NAME_ERROR: {SquadPaths[squad].FormattedRef}";
                            }

                            if (units.Keys.Contains(name))
                                units[name]++;
                            else
                                units[name] = 1;

                            var squad_exp = squad.GetSquadExpPrice(FileSystem, SquadPaths[squad].FormattedRef.GetPath(), cl);

                            expSum += squad_exp;

                            unitExps[name] = squad_exp;

                        }
                    } else if(!string.IsNullOrEmpty(unit.Squad.FormattedRef))
                    {
                        SquadRPGStats stats;
                        try
                        {
                            stats = LoadSquadRPGStats(squadSer, FileSystem, unit);
                        } catch(Exception ex)
                        {
                            cl.WriteLine($"Error reading squad stats: {ex.Message}");
                            continue;
                        }
                        string name = stats.GetUnitName(FileSystem, currentPath: unit.Squad.FormattedRef.GetPath(), logger: cl);
                        if (name == null)
                            name = $"NAME_ERROR: {unit.Squad.FormattedRef}";
                        name = name.Trim();
                        float squadExpPrice = stats.GetSquadExpPrice(FileSystem, unit.Squad.FormattedRef.GetPath(), cl);
                        expSum += squadExpPrice;
                        unitExps[name] = squadExpPrice;
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
                    sb.Append($" (ExpPrice: {unitExps[unit.Key]})");
                    sb.Append('\n');
                }
                sb.Append($"            ExpPriceSum: {expSum}\n");
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
                    unitExps.Clear();
                    expSum = 0f;
                    if (reinf.Entries.Items != null)
                    {
                        foreach (var unit in reinf.Entries.Items)
                        {
                            if (!string.IsNullOrEmpty(unit.MechUnit.FormattedRef))
                            {
                                MechUnitRPGStats stats = null;
                                try
                                {
                                    stats = LoadMechUnitRPGStats(mechStatsSer, FileSystem, unit);
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
                                if (name == null)
                                    name = $"NAME_ERROR: {unit.MechUnit.FormattedRef}";
                                name = name.Trim();
                                expSum += stats.ExpPrice;
                                unitExps[name] = stats.ExpPrice;
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
                                if (ArtyDB_Types.Keys.Contains(stats.DBtype) && stats.HasGunners)
                                {
                                    var squad = ArtyDB_Types[stats.DBtype];
                                    name = squad.GetUnitName(FileSystem, SquadPaths[squad].FormattedRef.GetPath(), logger: cl);

                                    if(string.IsNullOrWhiteSpace(name))
                                    {
                                        name = $"NAME_ERROR: {SquadPaths[squad].FormattedRef}";
                                    }

                                    if (units.Keys.Contains(name))
                                        units[name]++;
                                    else
                                        units[name] = 1;

                                    var squad_exp = squad.GetSquadExpPrice(FileSystem, SquadPaths[squad].FormattedRef.GetPath(), cl);

                                    expSum += squad_exp;

                                    unitExps[name] = squad_exp;

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
                                if (name == null)
                                    name = $"NAME_ERROR: {unit.Squad.FormattedRef}";
                                name = name.Trim();
                                float squadExpPrice = stats.GetSquadExpPrice(FileSystem, unit.Squad.FormattedRef.GetPath(), cl);
                                expSum += squadExpPrice;
                                unitExps[name] = squadExpPrice;
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
                        sb.Append($" (ExpPrice: {unitExps[unit.Key]})");
                        sb.Append('\n');
                    }
                    sb.Append($"                ExpPriceSum: {expSum}\n");
                }
                inx++;
            }
            fl.WriteLine(sb.ToString());
        }
        fl.Close();
    }

}