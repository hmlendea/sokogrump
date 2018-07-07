using System.IO;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;

namespace SokoGrump.GameLogic
{
    public class EditorEngine
    {
        readonly WorldManager worldManager;

        Tile[,] tiles;
        int width, height;
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
        /// Initializes a new instance of the <see cref="EditorEngine"/> class.
        /// </summary>
        public EditorEngine()
        {
            width = 16;
            height = 14;

            worldManager = new WorldManager();
        }

        public void LoadContent()
        {
            tiles = new Tile[width, height];

            worldManager.LoadContent();
        }

        public void UnloadContent()
        {
            tiles = null;

            worldManager.UnloadContent();
        }

        /// <summary>
        /// News the level.
        /// </summary>
        public void NewLevel()
        {
            tiles = new Tile[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[x, y] = worldManager.GetTile(7);
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
                        tiles[x, y] = worldManager.GetTile(0);
                    }
                    else if (id == 6)
                    {
                        plX = x;
                        plY = y;
                        tiles[x, y] = worldManager.GetTile(3);
                    }
                    else
                        tiles[x, y] = worldManager.GetTile(id);
                }
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
                        sw.Write(tiles[x, y].SpriteSheet);
                sw.WriteLine();
            }

            sw.Dispose();
        }

        /// <summary>
        /// Sets the tile.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="id">Identifier.</param>
        public void SetTile(int x, int y, int id)
        {
            tiles[x, y] = worldManager.GetTile(id);
        }

        /// <summary>
        /// Gets the tile I.
        /// </summary>
        /// <returns>The tile I.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public int GetTileID(int x, int y)
        {
            return tiles[x, y].Id;
        }
    }
}
