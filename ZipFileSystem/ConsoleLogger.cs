using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipFileSystem;


public class ConsoleLogger : ILogger
{
    public void WriteLine(string text, Color color)
    {
        Console.WriteLine(text);
    }

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}
