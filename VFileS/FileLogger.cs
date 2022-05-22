﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipFileSystem;

class FileLogger : ILogger
{

    FileStream f;
    StreamWriter sw;

    public FileLogger(string path)
    {
        f = File.Create(path);
        sw = new StreamWriter(f);
    }

    public void WriteLine(string text, Color color)
    {
        sw.WriteLine(text);
    }

    public void WriteLine(string text)
    {
        sw.WriteLine(text);
    }

    ~FileLogger()
    {
        sw.Close();
        f.Close();
    }
}
