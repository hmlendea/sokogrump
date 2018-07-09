using System.IO;

using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.GameLogic
{
    public class EditorEngine
    {
        readonly BoardManager worldManager;

        Board board;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorEngine"/> class.
        /// </summary>
        public EditorEngine()
        {
            worldManager = new BoardManager();
        }

        public void LoadContent()
        {
            board = new Board();

            worldManager.LoadContent();
        }

        public void UnloadContent()
        {
            board = null;

            worldManager.UnloadContent();
        }

        /// <summary>
        /// News the level.
        /// </summary>
        public void NewLevel()
        {
            board = new Board();

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    board.Tiles[x, y] = worldManager.GetTile(7);
                }
            }
        }

        /// <summary>
        /// Open the specified path.
        /// </summary>
        /// <param name="path">Path.</param>
        public void Open(string path)
        {
            string[] rows = File.ReadAllLines(path);

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    int id = (int)char.GetNumericValue(rows[y][x]);

                    if (id == 4)
                    {
                        board.PlayerStartLocation = new Point2D(x, y);
                        board.Tiles[x, y] = worldManager.GetTile(0);
                    }
                    else if (id == 6)
                    {
                        board.PlayerStartLocation = new Point2D(x, y);
                        board.Tiles[x, y] = worldManager.GetTile(3);
                    }
                    else
                    {
                        board.Tiles[x, y] = worldManager.GetTile(id);
                    }
                }
            }
        }

        /// <summary>
        /// Save the specified path.
        /// </summary>
        /// <param name="path">Path.</param>
        public void Save(string path)
        {
            StreamWriter sw = new StreamWriter(path);

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    if (board.PlayerStartLocation.X == x &&
                        board.PlayerStartLocation.Y == y)
                    {
                        sw.Write('4');
                    }
                    else
                    {
                        sw.Write(board.Tiles[x, y].SpriteSheet);
                    }
                }

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
            board.Tiles[x, y] = worldManager.GetTile(id);
        }

        /// <summary>
        /// Gets the tile I.
        /// </summary>
        /// <returns>The tile I.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public int GetTileID(int x, int y)
        {
            return board.Tiles[x, y].Id;
        }
    }
}
