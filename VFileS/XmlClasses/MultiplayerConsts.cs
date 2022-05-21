using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZipFileSystem
{

    [XmlRoot(ElementName = "MultiplayerConsts")]
    public class MultiplayerConsts
    {

        public class TechLevelsClass1
        {
            public class Item
            {
                [XmlElement(ElementName ="NameFileRef")]
                public FileRef NameFileRef;

                [XmlElement(ElementName = "DescriptionFileRef")]
                public FileRef DescriptionFileRef;
            }

            [XmlElement(ElementName ="Item")]
            public Item[] Items;

        }

        [XmlElement(ElementName ="TechLevels")]
        public TechLevelsClass1 TechLevels;

        public SidesClass Sides;

        public class SidesClass
        {

            [XmlElement(ElementName = "Item")]
            public ItemClass1[] Items;

            public class ItemClass1
            {

                public FileRef NameFileRef;
                public FileRef PartyInfo;
                public FileRef ListItemIcon;
                public string HistoricalSide;
                [XmlElement(ElementName = "TechLevels")]
                public TechLevelsClass2 TechLevels;

                //[XmlRoot(ElementName ="TechLevels")]
                public class TechLevelsClass2 
                {

                    [XmlElement(ElementName = "Item")]
                    public ItemClass2[] Items;

                    public class ItemClass2
                    {

                        //[XmlElement(ElementName = "Reinforcements")]
                        public ReinforcementsClass Reinforcements;
                        public FileRef StartingUnits;

                        public class ReinforcementsClass
                        {
                            [XmlElement(ElementName = "Item")]
                            public FileRef[] Items;
                        }
                    }

                    
                }

            }

        }

    }
}
