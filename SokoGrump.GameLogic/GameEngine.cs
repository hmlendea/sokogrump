using System;
using System.Collections.Generic;
using System.IO;

using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.GameLogic
{
    public class GameEngine
    {
        readonly BoardManager boardManager;

        Board board;
        Player player;
        int tableWidth, tableHeight, tileSize;
        int level, gameTime;
        bool isRunning;

        /// <summary>
        /// Gets the width of the table.
        /// </summary>
        /// <value>The width of the table.</value>
        public int TableWidth { get { return tableWidth; } }

        /// <summary>
        /// Gets the height of the table.
        /// </summary>
        /// <value>The height of the table.</value>
        public int TableHeight { get { return tableHeight; } }

        /// <summary>
        /// Gets the size of the tile.
        /// </summary>
        /// <value>The size of the tile.</value>
        public int TileSize { get { return tileSize; } }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get { return level; } }

        /// <summary>
        /// Gets the game time.
        /// </summary>
        /// <value>The game time.</value>
        public int GameTime { get { return gameTime; } }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        public bool IsRunning { get { return isRunning; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GameEngine"/> is completed.
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get { return board.TargetsLeft == 0; } }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine"/> class.
        /// </summary>
        public GameEngine()
        {
            tableWidth = 16;
            tableHeight = 14;
            tileSize = 48;
            
            boardManager = new BoardManager();
        }

        public void LoadContent()
        {
            boardManager.LoadContent();
        }

        public void UnloadContent()
        {
            boardManager.UnloadContent();
        }

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="level">Level.</param>
        public void NewGame(int level)
        {
            this.level = level;
            board = boardManager.GetBoard(level);

            player = new Player();
            gameTime = 0;
            isRunning = true;

            board.TargetsLeft = 0;

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    if (board.Tiles[x, y].Id == 3)
                    {
                        board.TargetsLeft += 1;
                    }
                }
            }

            player.Location = new Point2D(
                board.PlayerStartLocation.X,
                board.PlayerStartLocation.Y);
        }

        public void CheckCompletion()
        {
            if (Completed)
            {
                if (File.Exists(Path.Combine("Levels", (Level + 1) + ".lvl")))
                {
                    DateTime target2 = DateTime.Now.AddSeconds(0.25);
                    while (DateTime.Now <= target2)
                    {
                    }

                    NewGame(Level + 1);
                }
            }
        }

        bool TimerTick()
        {
            if (!isRunning)
                return true;

            gameTime += 1;

            return true;
        }

        /// <summary>
        /// Retry this instance.
        /// </summary>
        public void Retry()
        {
            NewGame(level);
        }

        /// <summary>
        /// Moves the player in a certain direction.
        /// </summary>
        /// <param name="direction">Movement direction.</param>
        public void MovePlayer(MovementDirection direction)
        {
            int dirX, dirY;
            int destX, destY;
            int dest2X, dest2Y;
            bool moved;

            switch (direction)
            {
                case MovementDirection.North:
                    dirX = 0;
                    dirY = -1;
                    break;

                case MovementDirection.West:
                    dirX = -1;
                    dirY = 0;
                    break;

                case MovementDirection.South:
                    dirX = 0;
                    dirY = 1;
                    break;

                case MovementDirection.East:
                    dirX = 1;
                    dirY = 0;
                    break;

                default:
                    return;
            }

            destX = player.Location.X + dirX;
            destY = player.Location.Y + dirY;
            dest2X = player.Location.X + dirX * 2;
            dest2Y = player.Location.Y + dirY * 2;

            moved = false;

            if (destX < 0 || destX >= tableWidth || destY < 0 || destY >= tableHeight)
                return;

            if (board.Tiles[destX, destY].TileType == TileType.Walkable)
                moved = true;
            else if (board.Tiles[destX, destY].TileType == TileType.Moveable)
            {
                if ((dirX < 0 && player.Location.X >= 2) || (dirX > 0 && player.Location.X < tableWidth - 2) ||
                    (dirY < 0 && player.Location.Y >= 2) || (dirY > 0 && player.Location.Y < tableHeight - 2))
                {
                    if (board.Tiles[destX, destY].Id == 2 || board.Tiles[destX, destY].Id == 5)
                    {
                        if (board.Tiles[dest2X, dest2Y].Id == 0)
                        {
                            board.Tiles[dest2X, dest2Y] = boardManager.GetTile(2);
                            board.Tiles[dest2X, dest2Y].Variation = board.Tiles[destX, destY].Variation;

                            if (board.Tiles[destX, destY].Id == 2)
                                board.Tiles[destX, destY] = boardManager.GetTile(0);
                            else
                            {
                                board.Tiles[destX, destY] = boardManager.GetTile(3);
                                board.TargetsLeft += 1;
                            }

                            moved = true;
                        }
                        else if (board.Tiles[dest2X, dest2Y].Id == 3)
                        {
                            board.Tiles[dest2X, dest2Y] = boardManager.GetTile(5);
                            board.Tiles[dest2X, dest2Y].Variation = board.Tiles[destX, destY].Variation;

                            board.TargetsLeft -= 1;

                            if (board.Tiles[destX, destY].Id == 2)
                                board.Tiles[destX, destY] = boardManager.GetTile(0);
                            else
                            {
                                board.Tiles[destX, destY] = boardManager.GetTile(3);
                                board.TargetsLeft += 1;
                            }

                            moved = true;
                        }
                    }
                }
            }

            if (dirX < 0)
            {
                player.Direction = MovementDirection.West;
            }
            else if (dirX > 0)
            {
                player.Direction = MovementDirection.East;
            }

            if (moved)
            {
                player.MovesCount += 1;
                player.Location = new Point2D(
                    player.Location.X + dirX,
                    player.Location.Y + dirY);
            }
        }

        public Player GetPlayer()
        {
            return player;
        }

        public Tile GetTile(int x, int y)
        {
            return board.Tiles[x, y];
        }

        public IEnumerable<Tile> GetTiles()
        {
            return boardManager.GetTiles();
        }

        int GetTileShape(int x, int y, int id)
        {
            bool w, e;

            if (x > 0)
                w = (board.Tiles[x - 1, y].Id == id);
            else
                w = false;

            if (x < tableWidth - 1)
                e = (board.Tiles[x + 1, y].Id == id);
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
