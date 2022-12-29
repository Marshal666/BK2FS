using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using CSharpImageLibrary;
using UsefulThings;
using System.Threading;

namespace DDS_MipMap_Fix
{

    public class DDSImage
    {

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No args given!");
                return;
            }
            var files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                if (file.EndsWith(".dds"))
                {
                    try
                    {
                        var data = File.ReadAllBytes(file);
                        DDSImage img = new DDSImage(data);
                        if (!img.HasMipMaps)
                        {
                            Console.WriteLine($"Texture {file} has no mipmaps!");
                            File.WriteAllBytes(file, img.FixMipMaps());
                        }
                        else
                        {
                            File.Delete(file);
                        }
                    } catch (Exception e)
                    {
                        Console.WriteLine("E: " + e.Message);
                    }
                }
            }
        }

        public static void CreateFile(string filePath, byte[] data)
        {
            if (!File.Exists(filePath))
            {
                var parent = Directory.GetParent(filePath);
                Directory.CreateDirectory(parent.FullName);
                using (var f = File.Create(filePath)) { f.WriteBytes(data); }
            }
        }

        ImageEngineImage img;

        public DDSImage(byte[] data)
        {
            img = new ImageEngineImage(data);
        }

        public bool HasMipMaps { get => img.NumMipMaps > 1; }

        public byte[] FixMipMaps()
        {
            var tex = img;
            ImageEngineFormat format = tex.Format;
            if (format != ImageEngineFormat.DDS_DXT1 || format != ImageEngineFormat.DDS_DXT3)
                format = ImageEngineFormat.DDS_DXT1;
            ImageFormats.ImageEngineFormatDetails d = new ImageFormats.ImageEngineFormatDetails(format);
            return tex.Save(d, MipHandling.GenerateNew, removeAlpha: false);
        }


    }

}
