using System;
using Gtk;
using SokoGrump.Game;

namespace SokoGrump
{
    public partial class EditorWindow : Window
    {
        EditorEngine editor;
        string fileName;
        int selectedTileID;

        public EditorWindow()
            : base(WindowType.Toplevel)
        {
            Build();
            daTable.DoubleBuffered = true;

            editor = new EditorEngine();
            editor.GdkWindowTable = daTable.GdkWindow;
            editor.NewLevel();

            selectedTileID = 0;

            daTable.ExposeEvent += delegate
            {
                editor.DrawTable();
            };
            daTable.AddEvents((int)Gdk.EventMask.ButtonPressMask);
        }

        protected void OnActTile7Activated(object sender, EventArgs e)
        {
            selectedTileID = 7;
        }

        protected void OnActTile1Activated(object sender, EventArgs e)
        {
            selectedTileID = 1;
        }

        protected void OnActTile0Activated(object sender, EventArgs e)
        {
            selectedTileID = 0;
        }

        protected void OnActTile3Activated(object sender, EventArgs e)
        {
            selectedTileID = 3;
        }

        protected void OnActTile2Activated(object sender, EventArgs e)
        {
            selectedTileID = 2;
        }

        protected void OnActTile5Activated(object sender, EventArgs e)
        {
            selectedTileID = 5;
        }

        protected void OnActTile4Activated(object sender, EventArgs e)
        {
            selectedTileID = 4;
        }

        protected void OnDaTableButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            int x = (int)args.Event.X / editor.TileSize;
            int y = (int)args.Event.Y / editor.TileSize;

            if (args.Event.Button == 1) // Left mouse click
            {
                if (selectedTileID == 4)
                {
                    editor.PlayerPosX = x;
                    editor.PlayerPosY = y;
                    editor.DrawTable();
                }
                else
                    editor.SetTile(x, y, selectedTileID);
            }
            else if (args.Event.Button == 3)  // Right mouse click
            {
                switch (editor.GetTileID(x, y))
                {
                    case 0:
                        editor.SetTile(x, y, 7);
                        break;

                    case 1:
                        editor.SetTile(x, y, 7);
                        break;
                    
                    case 2:
                        editor.SetTile(x, y, 0);
                        break;

                    case 3:
                        editor.SetTile(x, y, 0);
                        break;

                    case 5:
                        editor.SetTile(x, y, 0);
                        break;
                }
            }
        }

        protected void OnSaveActionActivated(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fileName))
                editor.Save(fileName);
            else
                saveAsAction.Activate();
        }

        protected void OnSaveAsActionActivated(object sender, EventArgs e)
        {
            FileChooserDialog fcd = new FileChooserDialog(
                                        "Save level as",
                                        this,
                                        FileChooserAction.Save,
                                        "Cancel", ResponseType.Cancel,
                                        "Save", ResponseType.Accept);
            fcd.SetCurrentFolder(Environment.CurrentDirectory);

            if (fcd.Run() == (int)ResponseType.Accept)
            {
                fileName = fcd.Filename;
                editor.Save(fileName);
            }

            fcd.Destroy();
        }

        protected void OnOpenActionActivated(object sender, EventArgs e)
        {
            FileChooserDialog fcd = new FileChooserDialog(
                                        "Open a level",
                                        this,
                                        FileChooserAction.Open,
                                        "Cancel", ResponseType.Cancel,
                                        "Open", ResponseType.Accept);
            fcd.SetCurrentFolder(Environment.CurrentDirectory);

            if (fcd.Run() == (int)ResponseType.Accept)
            {
                fileName = fcd.Filename;
                editor.Open(fileName);
            }

            fcd.Destroy();
        }

        protected void OnNewActionActivated(object sender, EventArgs e)
        {
            editor.NewLevel();
        }
    }
}
