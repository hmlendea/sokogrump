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
        int level;

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get { return level; } }

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

            if (destX < 0 || destX >= GameDefines.BoardWidth ||
                destY < 0 || destY >= GameDefines.BoardHeight)
            {
                return;
            }

            if (board.Tiles[destX, destY].TileType == TileType.Walkable)
            {
                moved = true;
            }
            else if (board.Tiles[destX, destY].TileType == TileType.Moveable)
            {
                if ((dirX < 0 && player.Location.X >= 2) || (dirX > 0 && player.Location.X < GameDefines.BoardWidth - 2) ||
                    (dirY < 0 && player.Location.Y >= 2) || (dirY > 0 && player.Location.Y < GameDefines.BoardHeight - 2))
                {
                    if (board.Tiles[destX, destY].Id == 2 || board.Tiles[destX, destY].Id == 5)
                    {
                        if (board.Tiles[dest2X, dest2Y].Id == 0)
                        {
                            board.Tiles[dest2X, dest2Y] = boardManager.GetTile(2);
                            board.Tiles[dest2X, dest2Y].Variation = board.Tiles[destX, destY].Variation;

                            if (board.Tiles[destX, destY].Id == 2)
                            {
                                board.Tiles[destX, destY] = boardManager.GetTile(0);
                            }
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
                            {
                                board.Tiles[destX, destY] = boardManager.GetTile(0);
                            }
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
            {
                w = (board.Tiles[x - 1, y].Id == id);
            }
            else
            {
                w = false;
            }

            if (x < GameDefines.BoardWidth - 1)
            {
                e = (board.Tiles[x + 1, y].Id == id);
            }
            else
            {
                e = false;
            }

            if (e && w)
            {
                return 0;
            }
            if (!e && w)
            {
                return 1;
            }
            if (e && !w)
            {
                return 2;
            }

            return 3;
        }
    }
}
