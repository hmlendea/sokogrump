using System.IO;
using System.Reflection;

using Gtk;
using SokoGrump.Windows;
using System;

namespace SokoGrump
{
    public class SokoGrumpGlobals
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

    class MainClass
    {
        public static void Main()
        {
            Application.Init();
            Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            GameWindow win = new GameWindow();
            win.Show();

            Application.Run();
        }
    }
}
