using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZipFileSystem
{
    public class Reinforcement
    {

        public string Type;

        public FileRef LocalizedNameFileRef;

        public EntriesClass Entries;

        public class EntriesClass
        {

            [XmlElement(ElementName = "Item")]
            public ItemClass[] Items;

            public class ItemClass
            {
                public FileRef MechUnit;
                public FileRef Squad;
            }
        }

    }
}
