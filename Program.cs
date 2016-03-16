using System.IO;
using System.Reflection;

using Gtk;

using SokoGrump.Windows;
using SokoGrump.Utils;

namespace SokoGrump
{
    class MainClass
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        public static void Main()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string binPath;

            binPath = Path.GetDirectoryName(thisAssembly.Location);

            Logger.MainLog.WriteLine(
                "Starting " + thisAssembly.GetName().Name +
                " v" + thisAssembly.GetName().Version);

            Directory.SetCurrentDirectory(binPath);


            Application.Init();

            GameWindow win = new GameWindow();
            win.Show();

            Application.Run();
        }
    }
}
