using SharpCompress.Archives.Zip;
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
    public static class Utils
    {

        public static string FullPath(this string s) => Path.GetFullPath(s).ToLower().Replace('\\', '/');

        public static string GetDirectory(this string s) => Path.GetDirectoryName(s).Replace('\\', '/');

        public static ZipArchiveEntry GetEntry(this ZipArchive a, string path)
        {
            //Slow linear approach, but since number of files is ~100k, it seems to be acceptable
            foreach(var e in a.Entries)
            {
                if (e.Key.ToLower() == path.ToLower())
                    return e;
            }
            return null;
        }

        public static byte[] GetBytes(this string s)
		{
            return Encoding.UTF8.GetBytes(s);
		}

        public static MemoryStream ToMemoryStream(this byte[] arr)
        {
            return new MemoryStream(arr);
        }

        public static string GetPath(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        static Dictionary<Type, XmlSerializer> Serializers = new Dictionary<Type, XmlSerializer>();
        public static object SerializeFromXMLString(this string str, Type type)
        {
            XmlSerializer ser;
            if (Serializers.ContainsKey(type))
                ser = Serializers[type];
            else
                ser = Serializers[type] = new XmlSerializer(type);
            object ret;
            using (var s = GenerateStreamFromString(str))
            {
                ret = ser.Deserialize(s);
            }
            return ret;
        }

        public static string FormattedPath(this string path)
        {
            return path.ToLower().Replace("\\", "/");
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static byte[] ReadAllBytes(this Stream source)
        {

            if (source == null)
                return null;

            long originalPosition = source.Position;
            if (source.Position != 0)
                source.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = source.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = source.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                try
                {
                    source.Position = originalPosition;
                }
                catch (Exception) { }
            }
        }

    }
}
