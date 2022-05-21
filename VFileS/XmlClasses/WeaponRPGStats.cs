using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZipFileSystem
{
	public class WeaponRPGStats
	{


		public string WeaponType;

		public float Dispersion;

		public float AimingTime;

		public float AmmoPerBurst;

		public float RangeMax;

		public float RangeMin;

		public float Ceiling;

		public float RevealRadius;

		public float DeltaAngle;

		public ShellsClass shells;

		public FileRef LocalizedNameFileRef;

		public class ShellsClass
		{

			[XmlElement(ElementName ="Item")]
			public ItemClass[] Items;

			public class ItemClass
			{
				public string DamageType;
				public float Piercing;
				public float DamageRandom;
				public float DamagePower;
				public float PiercingRandom;
				public float Area;
				public float Area2;
				public float Speed;
				public float BrokeTrackProbability;
				public float FireRate;
				public float RelaxTime;
			}

		}

	}
}
