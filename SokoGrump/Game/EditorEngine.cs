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

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get { return width; } }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get { return height; } }

        /// <summary>
        /// Gets the size of the tile.
        /// </summary>
        /// <value>The size of the tile.</value>
        public int TileSize { get { return tileSize; } }

        /// <summary>
        /// Gets or sets the player position x.
        /// </summary>
        /// <value>The player position x.</value>
        public int PlayerPosX
        {
            get { return plX; }
            set { plX = value; }
        }

        /// <summary>
        /// Gets or sets the player position y.
        /// </summary>
        /// <value>The player position y.</value>
        public int PlayerPosY
        {
            get { return plY; }
            set { plY = value; }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>The color of the foreground.</value>
        public Color ForegroundColor
        {
            get { return fgColor; }
            set { fgColor = value; }
        }

        /// <summary>
        /// Gets or sets the gdk window table.
        /// </summary>
        /// <value>The gdk window table.</value>
        public Gdk.Window GdkWindowTable
        {
            get { return gdkWindowTable; }
            set { gdkWindowTable = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorEngine"/> class.
        /// </summary>
        public EditorEngine()
        {
            width = 16;
            height = 14;
            tileSize = 48;

            bgColor = Color.Black;
            fgColor = Color.White;
        }

        /// <summary>
        /// News the level.
        /// </summary>
        public void NewLevel()
        {
            tiles = new Tile[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[x, y] = Tiles.ByID(7);

            DrawTable();
        }

        /// <summary>
        /// Open the specified path.
        /// </summary>
        /// <param name="path">Path.</param>
        public void Open(string path)
        {
            string[] rows = File.ReadAllLines(path);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int id = (int)char.GetNumericValue(rows[y][x]);

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

        /// <summary>
        /// Save the specified path.
        /// </summary>
        /// <param name="path">Path.</param>
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

        /// <summary>
        /// Draws the table.
        /// </summary>
        public void DrawTable()
        {
            Graphics g = Gtk.DotNet.Graphics.FromDrawable(gdkWindowTable);
            int x, y;

            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                {
                    g.DrawImage(
                        new Bitmap(Path.Combine("Tiles", "tile" + tiles[x, y].ID, "0.png")),
                        new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));

                    if (x == plX && y == plY)
                        g.DrawImage(
                            new Bitmap(Path.Combine("Tiles", "player", "player.png")),
                            new Rectangle(plX * tileSize, plY * tileSize, tileSize, tileSize));
                }
            g.Dispose();
        }

        /// <summary>
        /// Sets the tile.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="id">Identifier.</param>
        public void SetTile(int x, int y, int id)
        {
            tiles[x, y] = Tiles.ByID(id);
            DrawTable();
        }

        /// <summary>
        /// Gets the tile I.
        /// </summary>
        /// <returns>The tile I.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public int GetTileID(int x, int y)
        {
            return tiles[x, y].ID;
        }
    }
}
