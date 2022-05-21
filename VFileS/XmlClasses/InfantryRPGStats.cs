using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZipFileSystem
{
	public class InfantryRPGStats
	{

		public float MaxHP;

		public FileRef IconTexture;
		public FileRef LocalizedNameFileRef;

		public float Sight;
		public float SightPower;

		public float Speed;
		public float RotateSpeed;

		public float Passability;

		public FileRef Actions;

		/// <summary>
		/// AI Price
		/// </summary>
		public float Price;

		public float SmallAABBCoeff;

		public float ExpPrice;

		public GunsClass guns;

		public string SQLInsertTuple(string fileRef, IVirtualFileSystem fs, string currentPath = null, ILogger logger = null)
		{
			StringBuilder sb = new StringBuilder("(\'");
			sb.Append(fileRef);
			sb.Append("\', ");
			sb.Append(MaxHP);
			sb.Append(", \'");
			try
			{
				sb.Append(LocalizedNameFileRef.GetFileContents(fs, currentPath));
			}
			catch (Exception) { sb.Append("NO_NAME"); }
			sb.Append("\', ");
			sb.Append(Sight);
			sb.Append(", ");
			sb.Append(SightPower);
			sb.Append(", ");
			sb.Append(Speed);
			sb.Append(", ");
			sb.Append(RotateSpeed);
			sb.Append(", ");
			sb.Append(Passability);
			sb.Append(", \'");
			sb.Append(Actions.FormattedRef);
			sb.Append("\', ");
			sb.Append(Price);
			sb.Append(", ");
			sb.Append(SmallAABBCoeff);
			sb.Append(", ");
			sb.Append(ExpPrice);
			sb.Append(", \'{\'\'");
			for(int i = 0; i < guns?.Items?.Length; i++)
			{
				sb.Append((guns.Items[i].Ammo, guns.Items[i].Priority, guns.Items[i].ReloadConst).ToString());
				if (i + 1 != guns.Items.Length)
					sb.Append("\'\', \'\'");
				else
					sb.Append("\'\'");
			}
			sb.Append("}\', \'{");
			for (int i = 0; i < guns?.Items?.Length; i++)
			{
				sb.Append(guns.Items[i].Weapon.FormattedRef);
				if (i + 1 != guns.Items.Length)
					sb.Append("\'\', \'\'");
				else
					sb.Append("\'\'");
			}
			sb.Append("}\')");
			return sb.ToString();
		}

			public string GetName(IVirtualFileSystem fs, string rootPath)
		{
			return LocalizedNameFileRef.GetFileContents(fs, rootPath);
		}

		public (string path, WeaponRPGStats weapon)[] GetWeapons(IVirtualFileSystem fs, string rootDir = null, ILogger logs = null)
		{
			List<(string, WeaponRPGStats)> ret = new List<(string, WeaponRPGStats)>();

			if (guns == null || guns.Items == null)
				return ret.ToArray();
			foreach (var gun in guns.Items)
			{
				if (gun == null || gun.Weapon == null || gun.Weapon.href == null)
				{
					continue;
				}
				if (string.IsNullOrEmpty(gun.Weapon.href) || string.IsNullOrEmpty(gun.Weapon.href.Trim()))
					continue;
				try
				{
					string path = gun.Weapon.FormattedRef;
					string spath = path;
					if (!string.IsNullOrEmpty(path))
						path = path.GetDirectory();
					WeaponRPGStats weapon = (WeaponRPGStats)gun.Weapon.ReadXMLObject(typeof(WeaponRPGStats), fs, rootDir);
					ret.Add((spath, weapon));
					string name = weapon.LocalizedNameFileRef.GetFileContents(fs, path);
					logs?.WriteLine("Found soldier weapon: " + name);
				}
				catch (Exception e)
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

		public class GunsClass
		{

			[XmlElement(ElementName ="Item")]
			public ItemClass[] Items;

			public class ItemClass
			{

				public FileRef Weapon;
				public int Priority;
				public int Ammo;
				public int ReloadConst;

			}

		}
	}
}
