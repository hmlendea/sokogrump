using System;
using System.IO;
using System.Drawing;

namespace SokoGrump.Game
{
    public class EditorEngine
    {
        Tile[,] tiles;
        Gdk.Window gdkWindowTable;
        Color bgColor, fgColor;
        int width, height, tileSize;
        int plX, plY;

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public int TileSize { get { return tileSize; } }

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

        public Gdk.Window GdkWindowTable
        {
            get { return gdkWindowTable; }
            set { gdkWindowTable = value; }
        }

        public EditorEngine()
        {
            width = 16;
            height = 14;
            tileSize = 48;

            bgColor = Color.Black;
            fgColor = Color.White;
        }

        public void NewLevel()
        {
            tiles = new Tile[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[x, y] = Tiles.ByID(7);

            DrawTable();
        }

        public void Open(string path)
        {
            string[] rows = File.ReadAllLines(path);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int id = (int)Char.GetNumericValue(rows[y][x]);

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

            DrawTable();
        }

        public void Save(string path)
        {
            StreamWriter sw = new StreamWriter(path);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    if (PlayerPosX == x && PlayerPosY == y)
                        sw.Write('4');
                    else
                        sw.Write(tiles[x, y].ID);
                sw.WriteLine();
            }

            sw.Dispose();
        }

        public void DrawTable()
        {
            Graphics g = Gtk.DotNet.Graphics.FromDrawable(gdkWindowTable);
            int x, y;

            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                {
                    g.DrawImage(
                        new Bitmap(@"tiles/tile" + tiles[x, y].ID + "/0.png"),
                        new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));

                    if (x == plX && y == plY)
                        g.DrawImage(
                            new Bitmap(@"tiles/player/player.png"),
                            new Rectangle(plX * tileSize, plY * tileSize, tileSize, tileSize));
                }
            g.Dispose();
        }

        public void SetTile(int x, int y, int id)
        {
            tiles[x, y] = Tiles.ByID(id);
            DrawTable();
        }

        public int GetTileID(int x, int y)
        {
            return tiles[x, y].ID;
        }
    }
}
