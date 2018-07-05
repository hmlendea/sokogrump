using System;
using System.Collections.Generic;
using System.IO;

using NuciLog;
using NuciLog.Enumerations;

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
        Tile[,] tiles;
        PlayerDirection plD;
        int tableWidth, tableHeight, tileSize;
        int plX, plY;
        int level, moves, gameTime, targetsLeft;
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
        /// Gets the targets left.
        /// </summary>
        /// <value>The targets left.</value>
        public int TargetsLeft { get { return targetsLeft; } }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        public bool IsRunning { get { return isRunning; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GameEngine"/> is completed.
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get { return targetsLeft == 0; } }
        
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
        }

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="level">Level.</param>
        public void NewGame(int level)
        {
            this.level = level;
            Load(level);

            moves = 0;
            gameTime = 0;
            isRunning = true;
        }

        bool TimerTick()
        {
            if (!isRunning)
                return true;

            gameTime += 1;

            return true;
        }

        void GenerateVariations()
        {
            for (int y = 0; y < tableHeight; y++)
                for (int x = 0; x < tableWidth; x++)
                {
                    DirectoryInfo di = new DirectoryInfo(Path.Combine("Resources", "Tiles", "tile" + tiles[x, y].ID));
                    tiles[x, y].Variation = new Random().Next(0, di.GetFiles().Length);
                }
        }

        void Load(int level)
        {
            string levelFile = Path.Combine("Levels", level + ".lvl");
            string[] rows = File.ReadAllLines(levelFile);
            tiles = new Tile[tableWidth, tableHeight];
            targetsLeft = 0;

            for (int y = 0; y < tableHeight; y++)
                for (int x = 0; x < tableWidth; x++)
                {
                    int id = (int)char.GetNumericValue(rows[y][x]);

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

            LogManager.Instance.Info(
                Operation.WorldLoading,
                OperationStatus.Success,
                new Dictionary<LogInfoKey, string>()
                {
                    { LogInfoKey.FileName, levelFile }
                });
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

            if (tiles[destX, destY].Type == TileType.Transparent)
                moved = true;
            else if (tiles[destX, destY].Type == TileType.Moveable)
            {
                if ((dirX < 0 && plX >= 2) || (dirX > 0 && plX < tableWidth - 2) ||
                    (dirY < 0 && plY >= 2) || (dirY > 0 && plY < tableHeight - 2))
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
        }
        
        int GetTileShape(int x, int y, int id)
        {
            bool w, e;

            if (x > 0)
                w = (tiles[x - 1, y].ID == id);
            else
                w = false;

            if (x < tableWidth - 1)
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
