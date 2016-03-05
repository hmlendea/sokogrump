using System;
using System.IO;
using System.Drawing;

using SokoGrump.Game;

namespace SokoGrump.Windows
{
    public partial class GameWindow : Gtk.Window
    {
        GameEngine game;

        public GameWindow()
            : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            daTable.DoubleBuffered = false;

            game = new GameEngine();
            game.GdkWindowTable = daTable.GdkWindow;
            game.GdkWindowInfoBar = daInfoBar.GdkWindow;

            if (File.Exists("progress.sav"))
                Start(int.Parse(File.ReadAllText("progress.sav")));
            else
                Start(0);

            daTable.ExposeEvent += delegate
            {
                game.DrawTable();
            };
            daInfoBar.ExposeEvent += delegate
            {
                game.DrawInfoBar();
            };
        }

        protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
        {
            Gtk.Application.Quit();
            a.RetVal = true;
        }

        protected void OnKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
        {
            if (!game.IsRunning)
                return;

            bool validKeyPressed = false;
            switch (args.Event.Key)
            {
                case Gdk.Key.w:
                case Gdk.Key.W:
                case Gdk.Key.Up:
                    validKeyPressed = true;
                    game.MovePlayer(PlayerDirection.North);
                    break;

                case Gdk.Key.a:
                case Gdk.Key.A:
                case Gdk.Key.Left:
                    validKeyPressed = true;
                    game.MovePlayer(PlayerDirection.West);
                    break;

                case Gdk.Key.s:
                case Gdk.Key.S:
                case Gdk.Key.Down:
                    validKeyPressed = true;
                    game.MovePlayer(PlayerDirection.South);
                    break;

                case Gdk.Key.d:
                case Gdk.Key.D:
                case Gdk.Key.Right:
                    validKeyPressed = true;
                    game.MovePlayer(PlayerDirection.East);
                    break;

                case Gdk.Key.r:
                case Gdk.Key.R:
                    validKeyPressed = true;
                    game.Retry();
                    break;
            }

            if (validKeyPressed)
            {
                CheckCompletion();
            }
        }

        void Start(int level)
        {
            string path = System.IO.Path.Combine("Levels", level + ".lvl");

            if (File.Exists(path))
            {
                SaveProgress(level);
                game.NewGame(level);
            }
            else
                Console.WriteLine("Level not found! (" + path + ")");
        }

        void CheckCompletion()
        {
            if (game.Completed)
            {
                if (File.Exists(System.IO.Path.Combine("Levels", (game.Level + 1) + ".lvl")))
                {
                    DateTime target2 = DateTime.Now.AddSeconds(0.25);
                    while (DateTime.Now <= target2)
                    {
                    }
                    DateTime target = DateTime.Now.AddSeconds(1.5);
                    while (DateTime.Now <= target)
                    {
                        DrawLevelWon();
                    }

                    Start(game.Level + 1);
                }
                else
                    Destroy();
            }
        }

        void DrawLevelWon()
        {
            Graphics g = Gtk.DotNet.Graphics.FromDrawable(daTable.GdkWindow);
            g.DrawImage(Image.FromFile(System.IO.Path.Combine("Resources", "Images", "grumpy_cat_good.jpg")), 0, 0);
            g.Dispose();
        }

        static void SaveProgress(int level)
        {
            File.WriteAllText("progress.sav", level.ToString());
        }

        protected void OnActGoToActivated(object sender, EventArgs e)
        {
            game.NewGame(0);
        }

        protected void OnActRetryActivated(object sender, EventArgs e)
        {
            game.NewGame(game.Level);
        }

        protected void OnActEditorActivated(object sender, EventArgs e)
        {
            EditorWindow ew = new EditorWindow();
            ew.Show();
        }
    }
}

