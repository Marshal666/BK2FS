using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZipFileSystem
{
	public class MechUnitRPGStats
	{

		public float MaxHP;

		public FileRef IconTexture;
		public FileRef LocalizedNameFileRef;

		string nameCache = null;
		public string GetUnitName(IVirtualFileSystem fs, string currentPath = null, ILogger logger = null)
		{
			try
			{
				if (nameCache != null)
					return nameCache;
				nameCache = LocalizedNameFileRef.GetFileContents(fs, currentPath);
				return nameCache;
			} catch(Exception e)
			{
				logger?.WriteLine("Error: " + e.Message, Color.Red);
			}
			return null;
		}

		public float Sight;
		public float SightPower;

		public float Speed;
		public float RotateSpeed;

		public float Passability;

		/// <summary>
		/// AI Price coef
		/// </summary>
		public float Price;

		public float SmallAABBCoeff;

		public FileRef Actions;

		public float ExpPrice;

		public string UnitType;

		public PlatformsClass platforms;

		public ArmorsClass armors;

		///////////////////////////////////////////////////////
		///Plane related stuff...
		///
		public float MaxHeight;
		public float DivingAngle;
		public float ClimbAngle;
		public float TiltAngle;
		public float TiltRatio;
		public float TiltAcceleration;
		public float TiltSpeed;
		public float Fuel;
		public FileRef GAPAirAttackModifier;

		public float ReinforcementPrice;
		public float ReinforcementWeight;

		public FileRef InnerUnitBonus;

		public string SQLInsertTuple(string fileRef, IVirtualFileSystem fs, string currentPath = null, ILogger logger = null)
		{
			StringBuilder sb = new StringBuilder("(\'");
			sb.Append(fileRef);
			sb.Append("\', ");
			sb.Append(MaxHP);
			sb.Append(", ");
			sb.Append('\'');
			sb.Append(GetUnitName(fs, currentPath, logger));
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
			sb.Append(", ");
			sb.Append(Price);
			sb.Append(", ");
			sb.Append(SmallAABBCoeff);
			sb.Append(", \'");
			sb.Append(Actions.FormattedRef);
			sb.Append("\', ");
			sb.Append(ExpPrice);
			sb.Append(", \'");
			sb.Append(UnitType);
			sb.Append("\', \'{");
			for (int i = 0; i < armors?.Items?.Length; i++)
			{
				sb.Append(armors.Items[i].ToString());
				if(i + 1 != armors.Items.Length)
					sb.Append(", ");
			}
			sb.Append("}\', ");
			sb.Append(MaxHeight);
			sb.Append(", ");
			sb.Append(DivingAngle);
			sb.Append(", ");
			sb.Append(ClimbAngle);
			sb.Append(", ");
			sb.Append(TiltRatio);
			sb.Append(", ");
			sb.Append(TiltAcceleration);
			sb.Append(", ");
			sb.Append(TiltSpeed);
			sb.Append(", ");
			sb.Append(Fuel);
			sb.Append(", \'");
			sb.Append(GAPAirAttackModifier?.FormattedRef);
			sb.Append("\', ");
			sb.Append(ReinforcementPrice);
			sb.Append(", ");
			sb.Append(ReinforcementWeight);
			sb.Append(", \'");
			sb.Append(InnerUnitBonus?.FormattedRef);
			sb.Append("\'");
			sb.Append(')');
			return sb.ToString();
		}

		public (string, WeaponRPGStats)[] GetWeapons(IVirtualFileSystem fs, string rootDir = null, ILogger logs = null)
		{
			List<(string, WeaponRPGStats)> ret = new List<(string, WeaponRPGStats)>();
			if (platforms == null ||platforms.Items == null)
				return ret.ToArray();
			foreach(var item in platforms.Items)
			{
				if(item == null || item.guns == null || item.guns.Items == null)
				{
					continue;
				}
				foreach (var gun in item.guns.Items)
				{
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
						logs?.WriteLine("Found weapon: " + name);
					} catch (Exception e)
					{
						logs?.WriteLine("Error: " + e.Message, Color.Red);
					}
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

		public class PlatformsClass
		{

			[XmlElement(ElementName ="Item")]
			public Item1[] Items;

			public class Item1
			{
				public float HorizontalRotationSpeed;
				public float VerticalRotationSpeed;
				public int ParentPlatform;

				public GunsClass guns;

				public class GunsClass
				{

					[XmlElement(ElementName = "Item")]
					public Item2[] Items;

					public class Item2
					{
						public FileRef Weapon;
						public int Priority;
						public bool IsPrimary;
						public int Ammo;
						public float ReloadCost;
						public string ShootPoint;
					}

				}
			}

		}

		public class ArmorsClass
		{

			[XmlElement(ElementName = "Item")]
			public Item3[] Items;

			public class Item3
			{
				public float Min;
				public float Max;

				public override string ToString()
				{
					return $"{{{Min}, {Max}}}";
				}
			}

			

		}

	}
}
