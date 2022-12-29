using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZipFileSystem;

namespace VFileS.XmlClasses
{
    public class VisObj
    {
        public bool ForceAnimated;
        public Models Models;
    }

    public class Models
    {
        [XmlElement(ElementName = "Item")]
        public ModelsItem[] Items;
    }

    public class ModelsItem
    {
        public FileRef Model;
        public FileRef LowLevelModel;
        /// <summary>
        /// 6 possible values, each should always be present!:
        /// SEASON_WINTER, SEASON_SPRING, SEASON_SUMMER, SEASON_AUTUMN, SEASON_AFRICA, SEASON_ASIA
        /// </summary>
        public string Season;
    }
}
