using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK2_mod_translate_tool
{
    public static class Utils
    {

        static char[] InvalidPathChars = Path.GetInvalidPathChars();
        static char[] InvalidFileChars = Path.GetInvalidFileNameChars();

        public static string OriginalFolderName = "original";

        public static bool IsValidDirectoryName(string path)
        {
            foreach(var ch in InvalidPathChars)
            {
                if(path.Contains(ch))
                    return false;
            }
            foreach (var ch in InvalidFileChars)
            {
                if (path.Contains(ch))
                    return false;
            }
            if (path.ToLower() == OriginalFolderName)
                return false;
            if (string.IsNullOrEmpty(path.Trim()))
                return false;
            return true;
        }

        public static bool IsValidPath(string path, bool allowRelativePaths = true)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }

        public static bool PathExists(string path)
        {
            return Path.Exists(path);
        }

    }
}
