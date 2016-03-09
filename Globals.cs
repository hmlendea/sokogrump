using System.IO;
using System.Reflection;

namespace SokoGrump
{
    public static class Globals
    {
        /// <summary>
        /// Gets the bin path.
        /// </summary>
        /// <value>The bin path.</value>
        public static string BinPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        /// <summary>
        /// Gets the data path.
        /// </summary>
        /// <value>The data path.</value>
        public static string DataPath
        {
            get { return BinPath; }
        }
    }
}

