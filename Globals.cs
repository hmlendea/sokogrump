using System.IO;
using System.Reflection;

namespace SokoGrump
{
    public static class Globals
    {
        public static string BinPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        public static string DataPath
        {
            get { return BinPath; }
        }
    }
}

