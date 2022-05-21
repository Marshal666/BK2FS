using System.Drawing;

namespace ZipFileSystem
{
    public interface ILogger
    {

        void WriteLine(string text, Color color);

        void WriteLine(string text);

    }
}