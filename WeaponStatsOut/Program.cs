using System;
using ZipFileSystem;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using Fclp;

class Program
{

    public class Args
    {
        public string DataPath { get; set; }
        public string ModPath { get; set; }
        public string OutputPath { get; set; }
    }

    static void Main(string[] args)
    {
        var p = new FluentCommandLineParser<Args>();

        p.Setup(arg => arg.DataPath).As('d', "data").Required();
        p.Setup(arg => arg.ModPath).As('m', "mod");
        p.Setup(arg => arg.OutputPath).As('o', "out").Required();

        p.SetupHelp("data", "mod", "out");

        var res = p.Parse(args);

        if(res.HasErrors)
        {
            Console.WriteLine("Error parsing command line params!");
            Console.WriteLine(res.ErrorText);
            return;
        }

        Args options = p.Object;

        if(!Directory.Exists(options.OutputPath))
            Directory.CreateDirectory(options.OutputPath);

        FolderSystem fs = new FolderSystem(options.DataPath);
        ConsoleLogger cl = new ConsoleLogger();
        VirtualFileSystem FileSystem = new VirtualFileSystem(fs);
        PakLoader pk;
        FileSystem.AddSystem(pk = new PakLoader(options.DataPath, cl));
        if (!string.IsNullOrWhiteSpace(options.ModPath))
        {
            fs.AddDir(options.ModPath);
            pk.OpenDirectory(options.ModPath);
        }

        var f = FileSystem.ReadFileBytes("Consts/Test/Test_MultiplayerConsts.xdb");

        XmlSerializer mpConstsSer = new XmlSerializer(typeof(MultiplayerConsts));
        XmlSerializer reinfSer = new XmlSerializer(typeof(Reinforcement));
        XmlSerializer mechStatsSer = new XmlSerializer(typeof(MechUnitRPGStats));
        XmlSerializer squadSer = new XmlSerializer(typeof(SquadRPGStats));

        MultiplayerConsts data = (MultiplayerConsts)mpConstsSer.Deserialize(new MemoryStream(f));

        nint ix = 0;
        ix = 0;

        HashSet<string> DoneWeapons = new HashSet<string>();

        //fl.WriteLine("Units per Side/Tech level:");
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

                        //get weapons
                        (string, WeaponRPGStats)[] weapons = stats.GetWeapons(FileSystem, logs:cl);

                        foreach(var weapon in weapons)
                        {
                            if(!DoneWeapons.Contains(weapon.Item1))
                            {
                                DoneWeapons.Add(weapon.Item1);
                                WriteWeaponInfo(weapon.Item1, weapon.Item2);
                            }
                        }

                    }
                    else if (!string.IsNullOrEmpty(unit.Squad.FormattedRef))
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
                    ix = 0;
                }
                inx++;
            }
        }

        void WriteWeaponInfo(string weaponPath, WeaponRPGStats weapon)
        {
            string namePath = weapon.LocalizedNameFileRef.FormattedRef;
            string weaponName = weapon.LocalizedNameFileRef.GetFileContents(FileSystem, namePath, logger: cl);

            string outputPath = Path.Combine(options.OutputPath, namePath);
            StringBuilder sb = new StringBuilder();
            sb.Append(weaponName);
            sb.Append(Environment.NewLine);
            sb.Append($"Range: {weapon.RangeMax}");
            sb.Append(Environment.NewLine);
            sb.Append($"Dispersion: {weapon.Dispersion}");
            sb.Append(Environment.NewLine);
            sb.Append($"Aim time: {weapon.AimingTime} s");
            sb.Append(Environment.NewLine);
            if(weapon.AmmoPerBurst != 1)
            {
                sb.Append($"Volley shots: {weapon.AmmoPerBurst}");
                sb.Append(Environment.NewLine);
            }
            if (weapon.shells.Items != null && weapon.shells.Items != null)
            {
                int ix = 1;
                foreach (var shell in weapon.shells.Items)
                {
                    if (shell.DamageType == "DAMAGE_FOG")
                        continue;
                    sb.Append($"Shell {ix++}");
                    sb.Append(Environment.NewLine);
                    sb.Append($"\tDamage:\t\t\t\t{shell.DamagePower} +- {shell.DamageRandom}");
                    sb.Append(Environment.NewLine);
                    sb.Append($"\tPercing:\t\t\t{shell.Piercing} + {shell.PiercingRandom}");
                    sb.Append(Environment.NewLine);
                    sb.Append($"\tStrong explosion area:\t\t{shell.Area}");
                    sb.Append(Environment.NewLine);
                    sb.Append($"\tSoft explosion area:\t\t{shell.Area2}");
                    sb.Append(Environment.NewLine);
                    sb.Append($"\tTracking chance:\t\t{shell.BrokeTrackProbability}%");
                    sb.Append(Environment.NewLine);
                    sb.Append($"\tReload time:\t\t\t{shell.RelaxTime} s");
                    sb.Append(Environment.NewLine);
                    if (weapon.AmmoPerBurst != 1)
                    {
                        sb.AppendLine($"\tRate of fire:\t\t{shell.FireRate}");
                    }
                }
            }
            string dir = Path.GetDirectoryName(outputPath);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(outputPath, sb.ToString());
        }

    }

}