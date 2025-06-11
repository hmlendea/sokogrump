using System;
using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.GameLogic.GameManagers
{
    public class GameManager : IGameManager
    {
        readonly BoardManager boardManager;

        Random random;

        Board board;
        Player player;

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GameEngine"/> is completed.
        /// </summary>
        /// <value><c>true</c> if completed; otherwise, <c>false</c>.</value>
        public bool Completed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine"/> class.
        /// </summary>
        public GameManager() => boardManager = new BoardManager();

        public void LoadContent()
        {
            random = new Random();
            boardManager.LoadContent();
        }

        public void UnloadContent() => boardManager.UnloadContent();

        public void Update(double elapsedMiliseconds)
        {
            Completed = board.Targets.All(targetLocation => board.Tiles[targetLocation.X, targetLocation.Y].Id.Equals(TileId.CrateOnGround));

            boardManager.Update(elapsedMiliseconds);
        }

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="level">Level.</param>
        public void NewGame(int level)
        {
            Level = level;
            board = boardManager.GetBoard(level);

            player = new Player
            {
                Location = board.PlayerStartLocation
            };

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    if (board.Tiles[x, y].Id.Equals(TileId.EmptyTarget))
                    {
                        board.Tiles[x, y] = boardManager.GetTile(0);
                    }

                    if (board.Tiles[x, y].Id.Equals(TileId.CrateOnTarget))
                    {
                        board.Tiles[x, y] = boardManager.GetTile(2);
                    }
                }
            }

            GenerateVariations();
        }

        /// <summary>
        /// Retry this instance.
        /// </summary>
        public void Retry() => NewGame(Level);

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

            if (board.Tiles[destX, destY].TileType.Equals(TileType.Walkable))
            {
                moved = true;
            }
            else if (board.Tiles[destX, destY].TileType.Equals(TileType.Moveable))
            {
                if ((dirX < 0 && player.Location.X >= 2) || (dirX > 0 && player.Location.X < GameDefines.BoardWidth - 2) ||
                    (dirY < 0 && player.Location.Y >= 2) || (dirY > 0 && player.Location.Y < GameDefines.BoardHeight - 2))
                {
                    // If it's a crate
                    if (board.Tiles[destX, destY].Id.Equals(TileId.CrateOnGround))
                    {
                        if (board.Tiles[dest2X, dest2Y].Id.Equals(TileId.Ground))
                        {
                            int variation = board.Tiles[destX, destY].Variation;
                            board.Tiles[destX, destY] = boardManager.GetTile(0);
                            board.Tiles[dest2X, dest2Y] = boardManager.GetTile(2);
                            board.Tiles[dest2X, dest2Y].Variation = variation;

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

        public List<Point2D> GetTargets() => board.Targets;

        public Player GetPlayer() => player;

        public Tile GetTile(int x, int y) => board.Tiles[x, y];

        public IEnumerable<Tile> GetTiles() => boardManager.GetTiles();

        void GenerateVariations()
        {
            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    if (board.Tiles[x, y].Id.Equals(TileId.CrateOnGround))
                    {
                        board.Tiles[x, y].Variation = random.Next(0, 11);
                    }
                }
            }
        }
    }
}
