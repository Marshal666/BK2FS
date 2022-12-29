using System;
using ZipFileSystem;
using System.Reflection;
using System.Text;
using VFileS.XmlClasses;

class Program
{

    public static HashSet<string> IgnoreList = new HashSet<string>() 
    { 
        "IconTexture",
        "GAPAirAttackModifier",
        "InnerUnitBonus"
    };

    public static HashSet<string> ModelCheck = new HashSet<string>()
    {
        "VisObj",
        "visualObject",
        "AnimableModel",
        "TransportableModel"
    };

    public static HashSet<string> PrioModelCheck = new HashSet<string>()
    {
        "VisObj",
        "visualObject",
    };

    public static string[] Seasons = new string[] { "SEASON_WINTER", "SEASON_SPRING", "SEASON_SUMMER", "SEASON_AUTUMN", "SEASON_AFRICA", "SEASON_ASIA" };

    public static HashSet<string> Messages = new HashSet<string>();

    public static void Main(string[] args)
    {
        if (args.Length <= 0)
        {
            Console.WriteLine("No args given!");
            return;
        }
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

        var f = FileSystem.ReadTextFile("Consts/Test/Test_MultiplayerConsts.xdb");

        MultiplayerConsts data = (MultiplayerConsts)ZipFileSystem.Utils.SerializeFromXMLString(f, typeof(MultiplayerConsts));

        foreach (var side in data.Sides.Items)
        {
            int inx = 0;
            foreach (var year in data.TechLevels.Items)
            {
                var level = side.TechLevels.Items[inx];
                var startingUnits = (Reinforcement)ZipFileSystem.Utils.SerializeFromXMLString(level.StartingUnits.GetFileContents(FileSystem), typeof(Reinforcement));
                CheckReinforcement(startingUnits, level.StartingUnits.FormattedRef);
                foreach(var reinf in level.Reinforcements.Items)
                {
                    var r = (Reinforcement)reinf.GetFileContents(FileSystem).SerializeFromXMLString(typeof(Reinforcement));
                    CheckReinforcement(r, level.StartingUnits.FormattedRef);
                }
                inx++;
            }
        }

        foreach(var message in Messages)
        {
            Console.WriteLine(message);
        }

        void CheckReinforcement(Reinforcement r, string file)
        {
            if(r.Entries.Items == null)
            {
                Messages.Add($"WARNING: empty reinfocement items {file}");
                return;
            }
            foreach (var unit in r.Entries.Items)
            {
                if (unit.IsMechUnit)
                {
                    MechUnitRPGStats stats = (MechUnitRPGStats)FileSystem.ReadTextFile(unit.MechUnit.FormattedRef).SerializeFromXMLString(typeof(MechUnitRPGStats));
                    string root = unit.MechUnit.FormattedRef.FormattedPath().GetPath();
                    CheckMechUnitRPGStats(stats, unit.MechUnit.FormattedRef, root);
                }

            }
        }

        void CheckMechUnitRPGStats(MechUnitRPGStats stats, string file, string rootPath=null)
        {
            var hrefs = stats.Refs();
            foreach (var prop in hrefs)
            {
                if (IgnoreList.Contains(prop.Key))
                    continue;
                var href = prop.Value;
                if (ModelCheck.Contains(prop.Key))
                {
                    if(PrioModelCheck.Contains(prop.Key))
                    {
                        if(MissingRef(href.FormattedRef))
                        {
                            Messages.Add($"ERROR: Empty refrence filed in {file}: {prop.Key}");
                            continue;
                        }
                        string fr = href.FormattedRef;
                        if (rootPath != null && !FileSystem.ContainsFile(fr))
                            fr = Path.Combine(rootPath, fr);
                        if(!FileSystem.ContainsFile(fr))
                        {
                            Messages.Add($"ERROR: {file} invalid reference file \"{href.FormattedRef}\" of {prop.Key}");
                            continue;
                        }
                        VisObj vo = (VisObj)FileSystem.ReadTextFile(fr).SerializeFromXMLString(typeof(VisObj));
                        CheckVisObj(vo, fr, prop.Key, fr.GetPath());
                    } else
                    {
                        if (MissingRef(href.FormattedRef))
                        {
                            continue;
                        }
                        string fr = href.FormattedRef;
                        if (rootPath != null && !FileSystem.ContainsFile(fr))
                            fr = Path.Combine(rootPath, fr);
                        if (!FileSystem.ContainsFile(fr))
                        {
                            Messages.Add($"ERROR: {file} invalid reference file \"{href.FormattedRef}\" of {prop.Key}");
                            continue;
                        }
                        VisObj vo = (VisObj)FileSystem.ReadTextFile(fr).SerializeFromXMLString(typeof(VisObj));
                        CheckVisObj(vo, fr, prop.Key, fr.GetPath());
                    }
                    continue;
                }
                if(MissingRef(href.FormattedRef))
                {
                    Messages.Add($"ERROR: Empty refrence filed in {file}: {prop.Key}");
                }
                else if (!(FileSystem.ContainsFile(href.FormattedRef) || rootPath != null && FileSystem.ContainsFile(Path.Join(rootPath, href.FormattedRef))))
                {
                    Messages.Add($"ERROR: {file} invalid reference file \"{href.FormattedRef}\" of {prop.Key}");
                }

            }
        }

        void CheckVisObj(VisObj vo, string file, string prop, string root=null)
        {
            if(vo.Models == null || vo.Models.Items == null)
            {
                Messages.Add($"ERROR: {file} no models given!");
                return;
            }
            if(vo.Models.Items.Length != 6)
            {
                Messages.Add($"ERROR: {file} incorrect amount of models - {vo.Models.Items.Length} (not 6)!");
                return;
            }
            HashSet<string> seasonchk = new HashSet<string>(Seasons);
            foreach(var mi in vo.Models.Items)
            {
                seasonchk.Remove(mi.Season);
                if(MissingRef(mi.Model.FormattedRef))
                {
                    Messages.Add($"ERROR: {file} missing reference for season {mi.Season}");
                }
                if(InvalidRef(mi.Model.FormattedRef, FileSystem, root))
                {
                    Messages.Add($"ERROR: {file}-{prop} invalid reference for season {mi.Season} - missing file");
                }
                //Check model itself
            }
            if(seasonchk.Count != 0)
            {
                Messages.Add($"Invalid seasons in {file}");
            }
        }
    }

    public static bool MissingRef(string refs)
    {
        return string.IsNullOrEmpty(refs);
    }

    public static bool InvalidRef(string refs, IVirtualFileSystem fs, string root=null)
    {
        return !(fs.ContainsFile(refs) || root != null && fs.ContainsFile(Path.Combine(root, refs)));
    }

    

}

public static class Utils
{

    

    public static Dictionary<string, FileRef> Refs(this object o)
    {
        Dictionary<string, FileRef> ret = new Dictionary<string, FileRef>();
        foreach(var field in o.GetType().GetFields())
        {
            if(field.FieldType == typeof(FileRef))
            {
                ret.Add(field.Name, (FileRef)field.GetValue(o));
            }
        }
        return ret;
    }

}