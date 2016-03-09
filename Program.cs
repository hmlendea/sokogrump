using Gtk;
using SokoGrump.Windows;
using System;

namespace SokoGrump
{
    class MainClass
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        public static void Main()
        {
            Application.Init();

            GameWindow win = new GameWindow();
            win.Show();

            Application.Run();
        }
    }
}
