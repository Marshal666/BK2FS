using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpImageLibrary.DDS;
using CSharpImageLibrary;
using System.Drawing.Imaging;
using ZipFileSystem;

namespace ZipFileSystem
{
	public class Texture
	{

		public FileRef DestName;

		public int Width;
		public int Height;

		public byte[] GetImageBytes(IVirtualFileSystem fs, string rootDir, ILogger logs = null)
		{
			try
			{
				return DestName.GetFileContentsBin(fs, rootDir);
			}
			catch (Exception e)
			{
				logs?.WriteLine("Error: " + e.Message, Color.Red);
			}
			return null;
		}

	}
}
