﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ZipFileSystem
{
    public class FileRef
    {

        [XmlAttribute("href")]
        public string href;

        public bool RefFileExists(IVirtualFileSystem fs, string rootDir=null, bool removeExt=true) {
            if (string.IsNullOrEmpty(href) || string.IsNullOrEmpty(href.Trim()))
                return false;
            string hreff = (href.Trim()[0] == '\\' || href.Trim()[0] == '/' ? href.Substring(1) : href).Replace("\\", "/");
            if (removeExt)
            {
                int inx = hreff.LastIndexOf('#');
                if (inx > -1)
                    hreff = hreff.Substring(0, hreff.Length - (hreff.Length - inx));
            }
            hreff = hreff.Replace('\\', '/');
            string dir = Path.Combine(rootDir, hreff).Replace("\\", "/");
            if (fs.ContainsFile(dir))
                return true;
            if (fs.ContainsFile(hreff))
                return true;
            return false;
        }

        string fc = null;
        public string GetFileContents(IVirtualFileSystem fs, string rootDir = null, bool removeExt=true, ILogger? logger = null)
        {
            if (fc != null)
                return fc;
            if (rootDir == null)
                rootDir = "";
            //rootDir = Directory.GetCurrentDirectory().Replace("\\", "/");
            if (string.IsNullOrEmpty(href) || string.IsNullOrEmpty(href.Trim()))
                return null;
            string hreff = (href.Trim()[0] == '\\' || href.Trim()[0] == '/' ? href.Substring(1) : href).Replace("\\", "/");
            if(removeExt)
            {
                int inx = hreff.LastIndexOf('#');
                if(inx > -1)
                    hreff = hreff.Substring(0, hreff.Length - (hreff.Length - inx));
            }
            hreff = hreff.Replace('\\', '/').ToLower();
            string dir = Path.Combine(rootDir.ToLower(), hreff).Replace("\\", "/");
            try
            {
                fc = fs.ReadTextFile(dir);
            } catch(Exception e)
            {
                logger?.WriteLine("Error: " + e.Message);
            }
            if (fc != null)
                return fc;
            logger?.WriteLine($"Failed getting file on path: {dir}, it might not exist!");
            try
            {
                fc = fs.ReadTextFile(hreff);
            } catch(Exception e)
            {
                logger?.WriteLine("Error: " + e.Message);
            }
            if (fc != null)
                return fc;
            throw new FileNotFoundException(hreff);
        }

        byte[] binfc = null;
        public byte[] GetFileContentsBin(IVirtualFileSystem fs, string? rootDir = null, bool removeExt = true, ILogger? logger = null)
        {
            if (binfc != null)
                return binfc;
            if (rootDir == null)
                rootDir = "";
            //rootDir = Directory.GetCurrentDirectory().Replace("\\", "/");
            if (string.IsNullOrEmpty(href) || string.IsNullOrEmpty(href.Trim()))
                return null;
            string hreff = (href.Trim()[0] == '\\' || href.Trim()[0] == '/' ? href.Substring(1) : href).Replace("\\", "/");
            if (removeExt)
            {
                int inx = hreff.LastIndexOf('#');
                if (inx > -1)
                    hreff = hreff.Substring(0, hreff.Length - (hreff.Length - inx));
            }
            hreff = hreff.Replace('\\', '/').ToLower();
            string dir = Path.Combine(rootDir.ToLower(), hreff).Replace("\\", "/");
            binfc = fs.ReadFileBytes(dir);
            if (binfc != null)
                return binfc;
            logger?.WriteLine($"Failed getting file on path: {dir}, it might not exist!");
            binfc = fs.ReadFileBytes(hreff);
            if (binfc != null)
                return binfc;
            throw new FileNotFoundException(hreff);
        }

        public string FormattedRef
		{
			get
			{
                if (string.IsNullOrEmpty(href) || string.IsNullOrEmpty(href.Trim()))
                    return string.Empty;
                string hreff = (href.Trim()[0] == '\\' || href.Trim()[0] == '/' ? href.Substring(1) : href).Replace("\\", "/");
                int inx = hreff.LastIndexOf('#');
                if (inx > -1)
                    hreff = hreff.Substring(0, hreff.Length - (hreff.Length - inx));
                return hreff.Replace('\\', '/');
            }
		}

        public object ReadXMLObject(Type type, IVirtualFileSystem fs, string rootDir = null, bool removeExt=true)
		{
            return GetFileContents(fs, rootDir, removeExt).SerializeFromXMLString(type);
		}

    }
}
