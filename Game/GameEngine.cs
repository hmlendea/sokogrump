using System;
using System.IO;
using System.Drawing;

namespace SokoGrump.Game
{
    public enum PlayerDirection
    {
        North,
        West,
        South,
        East
    }

    public class GameEngine
    {
        Tile[,] tiles;
        PlayerDirection plD;
        Gdk.Window gdkWindowTable, gdkWindowInfoBar;
        Color bgColor, fgColor;
        int width, height, tileSize;
        int plX, plY;
        int level, moves, gameTime, targetsLeft;
        bool isRunning;

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public int TileSize { get { return tileSize; } }

        public PlayerDirection PlayerDirection { get { return plD; } }

        public int Level { get { return level; } }

        public int GameTime { get { return gameTime; } }

        public int Moves { get { return moves; } }

        public int TargetsLeft { get { return targetsLeft; } }

        public bool IsRunning { get { return isRunning; } }

        public bool Completed { get { return targetsLeft == 0; } }

        public Color BackgroundColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }

        public Color ForegroundColor
        {
            get { return fgColor; }
            set { fgColor = value; }
        }

        public int PlayerPosX
        {
            get { return plX; }
            set { plX = value; }
        }

        public int PlayerPosY
        {
            get { return plY; }
            set { plY = value; }
        }


        public Gdk.Window GdkWindowTable
        {
            get { return gdkWindowTable; }
            set { gdkWindowTable = value; }
        }

        public Gdk.Window GdkWindowInfoBar
        {
            get { return gdkWindowInfoBar; }
            set { gdkWindowInfoBar = value; }
        }

        public GameEngine()
        {
            width = 16;
            height = 14;
            tileSize = 48;

            plD = PlayerDirection.North;
            plX = 0;
            plY = 0;

            bgColor = Color.Black;
            fgColor = Color.White;

            GLib.Timeout.Add(1000, new GLib.TimeoutHandler(TimerTick));
        }

        public void NewGame(int level)
        {
            this.level = level;
            Load(level);

            moves = 0;
            gameTime = 0;
            isRunning = true;

            DrawTable();
            DrawInfoBar();
        }

        bool TimerTick()
        {
            if (!isRunning)
                return true;
            
            gameTime += 1;
            DrawInfoBar();

            return true;
        }

        void GenerateVariations()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    DirectoryInfo di = new DirectoryInfo(Path.Combine(
                                               Globals.DataPath, "Resources", "Tiles", "tile" + tiles[x, y].ID));
                    tiles[x, y].Variation = new Random().Next(0, di.GetFiles().Length);
                }
        }

        void Load(int level)
        {
            string[] rows = File.ReadAllLines(Path.Combine(Globals.DataPath, "Levels", level + ".lvl"));
            tiles = new Tile[width, height];
            targetsLeft = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int id = (int)Char.GetNumericValue(rows[y][x]);

                    if (id == 3)
                        targetsLeft += 1;
                    
                    if (id == 4)
                    {
                        plX = x;
                        plY = y;
                        tiles[x, y] = Tiles.ByID(0);
                    }
                    else if (id == 6)
                    {
                        plX = x;
                        plY = y;
                        tiles[x, y] = Tiles.ByID(3);
                    }
                    else
                        tiles[x, y] = Tiles.ByID(id);
                }
            GenerateVariations();

            Console.WriteLine("Level " + level + " loaded");
        }

        public void Retry()
        {
            NewGame(level);
        }

        public void MovePlayer(PlayerDirection playerDirection)
        {
            int dirX, dirY;
            int destX, destY;
            int dest2X, dest2Y;
            bool moved;

            switch (playerDirection)
            {
                case PlayerDirection.North:
                    dirX = 0;
                    dirY = -1;
                    break;

                case PlayerDirection.West:
                    dirX = -1;
                    dirY = 0;
                    break;

                case PlayerDirection.South:
                    dirX = 0;
                    dirY = 1;
                    break;

                case PlayerDirection.East:
                    dirX = 1;
                    dirY = 0;
                    break;

                default:
                    return;
            }

            destX = plX + dirX;
            destY = plY + dirY;
            dest2X = plX + dirX * 2;
            dest2Y = plY + dirY * 2;

            moved = false;

            if (destX < 0 || destX >= width || destY < 0 || destY >= height)
                return;

            if (tiles[destX, destY].Type == TileType.Transparent)
                moved = true;
            else if (tiles[destX, destY].Type == TileType.Moveable)
            {
                if ((dirX < 0 && plX >= 2) || (dirX > 0 && plX < width - 2) ||
                    (dirY < 0 && plY >= 2) || (dirY > 0 && plY < height - 2))
                {
                    if (tiles[destX, destY].ID == 2 || tiles[destX, destY].ID == 5)
                    {
                        if (tiles[dest2X, dest2Y].ID == 0)
                        {
                            tiles[dest2X, dest2Y] = Tiles.ByID(2);
                            tiles[dest2X, dest2Y].Variation = tiles[destX, destY].Variation;

                            if (tiles[destX, destY].ID == 2)
                                tiles[destX, destY] = Tiles.ByID(0);
                            else
                            {
                                tiles[destX, destY] = Tiles.ByID(3);
                                targetsLeft += 1;
                            }

                            moved = true;
                        }
                        else if (tiles[dest2X, dest2Y].ID == 3)
                        {
                            tiles[dest2X, dest2Y] = Tiles.ByID(5);
                            tiles[dest2X, dest2Y].Variation = tiles[destX, destY].Variation;

                            targetsLeft -= 1;

                            if (tiles[destX, destY].ID == 2)
                                tiles[destX, destY] = Tiles.ByID(0);
                            else
                            {
                                tiles[destX, destY] = Tiles.ByID(3);
                                targetsLeft += 1;
                            }

                            moved = true;
                        }
                    }
                }
            }
                
            if (dirX < 0)
                plD = PlayerDirection.West;
            else if (dirX > 0)
                plD = PlayerDirection.East;

            if (moved)
            {
                plX += dirX;
                plY += dirY;
                moves += 1;
            }

            DrawTable();
            DrawInfoBar();
        }

        public void DrawTable()
        {
            if (!isRunning)
                return;
            
            Graphics g = Gtk.DotNet.Graphics.FromDrawable(gdkWindowTable);
            int x, y;

            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                {
                    string resourceName;

                    if (tiles[x, y].ID == 1)
                        resourceName = Path.Combine(Globals.DataPath, "Resources", "Tiles", "tile1", GetTileShape(x, y, 1) + ".png");
                    else
                        resourceName = Path.Combine(Globals.DataPath, "Resources", "Tiles", "tile" + tiles[x, y].ID, tiles[x, y].Variation + ".png");

                    g.DrawImage(
                        new Bitmap(resourceName),
                        new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                    
                    if (x == plX && y == plY)
                    {
                        if (plD == PlayerDirection.West)
                            resourceName = Path.Combine(Globals.DataPath, "Resources", "Tiles", "player", "playerW.png");
                        else if (plD == PlayerDirection.East)
                            resourceName = Path.Combine(Globals.DataPath, "Resources", "Tiles", "player", "playerE.png");
                        else
                            resourceName = Path.Combine(Globals.DataPath, "Resources", "Tiles", "player", "player.png");

                        g.DrawImage(
                            new Bitmap(resourceName),
                            new Rectangle(plX * tileSize, plY * tileSize, tileSize, tileSize));
                    }
                }
            g.Dispose();
        }

        public void DrawInfoBar()
        {
            if (!isRunning)
                return;

            Graphics gfx = Gtk.DotNet.Graphics.FromDrawable(gdkWindowInfoBar);
            Brush brBg = new SolidBrush(bgColor);
            Brush brFg = new SolidBrush(fgColor);

            int w, h, w3;
            gdkWindowInfoBar.GetSize(out w, out h);
            w3 = w / 3;

            Rectangle recWhole = new Rectangle(0, 0, w, h);
            Rectangle recLeft = new Rectangle(0, 0, w3, h);
            Rectangle recMiddle = new Rectangle(w3, 0, w3, h);
            Rectangle recRight = new Rectangle(w3 * 2, 0, w3, h);

            Font f = new Font("Sans", (int)(Math.Min(w, h) * 0.5), FontStyle.Regular);
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            gfx.FillRectangle(brBg, recWhole);

            gfx.DrawString("Moves: " + moves, f, brFg, recLeft, strFormat);
            gfx.DrawString("Level " + level, f, brFg, recMiddle, strFormat);
            gfx.DrawString(string.Format("{0:00}:{1:00}", (gameTime / 60) % 60, gameTime % 60), f, brFg, recRight, strFormat);

            gfx.Dispose();
        }

        int GetTileShape(int x, int y, int id)
        {
            bool w, e;

            if (x > 0)
                w = (tiles[x - 1, y].ID == id);
            else
                w = false;

            if (x < width - 1)
                e = (tiles[x + 1, y].ID == id);
            else
                e = false;

            if (e && w)
                return 0;
            if (!e && w)
                return 1;
            if (e && !w)
                return 2;
            return 3;
        }
    }
}
