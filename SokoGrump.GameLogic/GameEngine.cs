using System;
using System.Collections.Generic;
using System.IO;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.GameLogic
{
    public enum PlayerDirection
    {
        /// <summary>
        /// The north.
        /// </summary>
        North,
        /// <summary>
        /// The west.
        /// </summary>
        West,
        /// <summary>
        /// The south.
        /// </summary>
        South,
        /// <summary>
        /// The east.
        /// </summary>
        East
    }

    public class GameEngine
    {
        readonly BoardManager boardManager;

        Board board;
        PlayerDirection plD;
        int tableWidth, tableHeight, tileSize;
        int plX, plY;
        int level, moves, gameTime;
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
        /// Gets the player direction.
        /// </summary>
        /// <value>The player direction.</value>
        public PlayerDirection PlayerDirection { get { return plD; } }

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
        /// Gets the moves.
        /// </summary>
        /// <value>The moves.</value>
        public int Moves { get { return moves; } }

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
        /// Initializes a new instance of the <see cref="GameEngine"/> class.
        /// </summary>
        public GameEngine()
        {
            tableWidth = 16;
            tableHeight = 14;
            tileSize = 48;

            plD = PlayerDirection.North;
            plX = 0;
            plY = 0;

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

            moves = 0;
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

            plX = board.PlayerStartLocation.X;
            plY = board.PlayerStartLocation.Y;
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
        /// Moves the player.
        /// </summary>
        /// <param name="playerDirection">Player direction.</param>
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

            if (destX < 0 || destX >= tableWidth || destY < 0 || destY >= tableHeight)
                return;

            if (board.Tiles[destX, destY].TileType == TileType.Walkable)
                moved = true;
            else if (board.Tiles[destX, destY].TileType == TileType.Moveable)
            {
                if ((dirX < 0 && plX >= 2) || (dirX > 0 && plX < tableWidth - 2) ||
                    (dirY < 0 && plY >= 2) || (dirY > 0 && plY < tableHeight - 2))
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
                plD = PlayerDirection.West;
            else if (dirX > 0)
                plD = PlayerDirection.East;

            if (moved)
            {
                plX += dirX;
                plY += dirY;
                moves += 1;
            }
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
