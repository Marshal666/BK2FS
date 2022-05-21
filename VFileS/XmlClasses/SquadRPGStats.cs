using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZipFileSystem
{
	public class SquadRPGStats
	{

		public float MaxHP;

		public FileRef IconTexture;

		public FileRef LocalizedNameFileRef;

		public string SquadType;

		public string type;

		public MembersClass members;

		public float EntrenchCover;

		public class MembersClass
		{

			[XmlElement(ElementName ="Item")]
			public FileRef[] Items;

		}

		public string GetUnitName(IVirtualFileSystem fs, string currentPath = null, ILogger logger = null)
		{
			try
			{
				return LocalizedNameFileRef.GetFileContents(fs, currentPath);
			}
			catch (Exception e)
			{
				logger?.WriteLine("Error: " + e.Message + " cpath: " + currentPath, Color.Red);
			}
			return null;
		}

		public (string path, InfantryRPGStats stats)[] GetInfantry(IVirtualFileSystem fs, string rootDir = null, ILogger logs = null)
		{
			List<(string path, InfantryRPGStats stats)> ret = new List<(string path, InfantryRPGStats stats)>();
			if (members == null || members.Items == null)
				return ret.ToArray();
			foreach(var member in members.Items)
			{
				try
				{
					if (string.IsNullOrEmpty(member.href) || string.IsNullOrEmpty(member.href.Trim()))
						continue;
					string path = member.FormattedRef;
					string spath = path;
					if (!string.IsNullOrEmpty(path))
						path = path.GetDirectory();
					InfantryRPGStats inf = (InfantryRPGStats)member.ReadXMLObject(typeof(InfantryRPGStats), fs);
					ret.Add((spath, inf));
					logs.WriteLine("Found infantry: " + inf.GetName(fs, path));
				} catch(Exception e)
				{
					logs?.WriteLine("Error: " + e.Message, Color.Red);
				}
			}
			return ret.ToArray();
		}

		public Texture GetIcon(IVirtualFileSystem fs, string rootDir = null, ILogger logs = null)
		{
			try
			{
				return (Texture)IconTexture.GetFileContents(fs, rootDir).SerializeFromXMLString(typeof(Texture));
			}
			catch (Exception e)
			{
				logs?.WriteLine("Error: " + e.Message, Color.Red);
			}
			return null;
		}

	}
}
