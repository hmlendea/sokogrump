using Gtk;
using SokoGrump.Windows;
using System;

namespace SokoGrump
{
    class MainClass
    {
        public static void Main()
        {
            Application.Init();

            GameWindow win = new GameWindow();
            win.Show();

            Application.Run();
        }
    }
}
