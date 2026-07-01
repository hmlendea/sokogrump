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

        readonly record struct MoveSnapshot(
            Point2D PlayerLocation,
            MovementDirection PlayerDirection,
            int MovesCount,
            int CrateFromX, int CrateFromY, Tile CrateFromTile,
            int CrateToX, int CrateToY, Tile CrateToTile,
            bool CratePushed);

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

        public TimeSpan ElapsedTime { get; private set; }

        public bool CanUndo => undoHistory.Count > 0;

        readonly Stack<MoveSnapshot> undoHistory = new();

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

            if (!Completed)
            {
                ElapsedTime += TimeSpan.FromMilliseconds(elapsedMiliseconds);
            }

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

            ElapsedTime = TimeSpan.Zero;

            player = new Player
            {
                Location = board.PlayerStartLocation,
                Direction = MovementDirection.South
            };

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    if (board.Tiles[x, y].Id.Equals(TileId.EmptyTarget))
                    {
                        board.Tiles[x, y] = boardManager.GetTile(TileId.Ground);
                    }

                    if (board.Tiles[x, y].Id.Equals(TileId.CrateOnTarget))
                    {
                        board.Tiles[x, y] = boardManager.GetTile(TileId.CrateOnGround);
                    }
                }
            }

            GenerateVariations();
            undoHistory.Clear();
        }

        /// <summary>
        /// Retry this instance.
        /// </summary>
        public void Retry() => NewGame(Level);

        public void SetPlayerDirection(MovementDirection direction)
            => player.Direction = direction;

        static DirectionDelta? GetDirectionDelta(MovementDirection direction) => direction switch
        {
            MovementDirection.North => new DirectionDelta(0, -1),
            MovementDirection.West  => new DirectionDelta(-1, 0),
            MovementDirection.South => new DirectionDelta(0, 1),
            MovementDirection.East  => new DirectionDelta(1, 0),
            _ => null
        };

        public bool CanMove(MovementDirection direction)
        {
            if (GetDirectionDelta(direction) is not DirectionDelta delta)
            {
                return false;
            }

            int dirX = delta.X;
            int dirY = delta.Y;

            int destX = player.Location.X + dirX;
            int destY = player.Location.Y + dirY;
            int dest2X = player.Location.X + dirX * 2;
            int dest2Y = player.Location.Y + dirY * 2;

            if (destX < 0 || destX >= GameDefines.BoardWidth ||
                destY < 0 || destY >= GameDefines.BoardHeight)
            {
                return false;
            }

            if (board.Tiles[destX, destY].TileType is TileType.Walkable)
            {
                return true;
            }

            if (board.Tiles[destX, destY].TileType is TileType.Moveable)
            {
                if ((dirX < 0 && player.Location.X >= 2) || (dirX > 0 && player.Location.X < GameDefines.BoardWidth - 2) ||
                    (dirY < 0 && player.Location.Y >= 2) || (dirY > 0 && player.Location.Y < GameDefines.BoardHeight - 2))
                {
                    if (board.Tiles[destX, destY].Id.Equals(TileId.CrateOnGround))
                    {
                        if (board.Tiles[dest2X, dest2Y].Id.Equals(TileId.Ground))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the player in a certain direction.
        /// </summary>
        /// <param name="direction">Movement direction.</param>
        public void MovePlayer(MovementDirection direction)
        {
            if (GetDirectionDelta(direction) is not DirectionDelta delta)
            {
                return;
            }

            if (!CanMove(direction))
            {
                return;
            }

            int dirX = delta.X;
            int dirY = delta.Y;
            int destX = player.Location.X + dirX;
            int destY = player.Location.Y + dirY;
            int dest2X = player.Location.X + dirX * 2;
            int dest2Y = player.Location.Y + dirY * 2;

            MoveSnapshot snapshot = CreateMoveSnapshot(destX, destY, dest2X, dest2Y);
            TryPushCrate(dirX, dirY, destX, destY, dest2X, dest2Y);
            player.Direction = direction;
            CommitMove(dirX, dirY, snapshot);
        }

        MoveSnapshot CreateMoveSnapshot(int destX, int destY, int dest2X, int dest2Y)
        {
            bool cratePushed = board.Tiles[destX, destY].TileType is TileType.Moveable;

            return new MoveSnapshot(
                player.Location,
                player.Direction,
                player.MovesCount,
                destX, destY, board.Tiles[destX, destY].Clone(),
                dest2X, dest2Y, board.Tiles[dest2X, dest2Y].Clone(),
                cratePushed);
        }

        void TryPushCrate(int dirX, int dirY, int destX, int destY, int dest2X, int dest2Y)
        {
            if (board.Tiles[destX, destY].TileType is not TileType.Moveable)
            {
                return;
            }

            bool canReachDest2 =
                (dirX < 0 && player.Location.X >= 2) ||
                (dirX > 0 && player.Location.X < GameDefines.BoardWidth - 2) ||
                (dirY < 0 && player.Location.Y >= 2) ||
                (dirY > 0 && player.Location.Y < GameDefines.BoardHeight - 2);

            if (!canReachDest2)
            {
                return;
            }

            if (!board.Tiles[destX, destY].Id.Equals(TileId.CrateOnGround) ||
                !board.Tiles[dest2X, dest2Y].Id.Equals(TileId.Ground))
            {
                return;
            }

            int variation = board.Tiles[destX, destY].Variation;
            board.Tiles[destX, destY] = boardManager.GetTile(TileId.Ground);
            board.Tiles[dest2X, dest2Y] = boardManager.GetTile(TileId.CrateOnGround);
            board.Tiles[dest2X, dest2Y].Variation = variation;
        }

        void CommitMove(int dirX, int dirY, MoveSnapshot snapshot)
        {
            player.MovesCount += 1;
            player.Location = new Point2D(
                player.Location.X + dirX,
                player.Location.Y + dirY);

            undoHistory.Push(snapshot);
        }

        public void Undo()
        {
            if (undoHistory.Count == 0)
            {
                return;
            }

            MoveSnapshot snapshot = undoHistory.Pop();

            if (snapshot.CratePushed)
            {
                board.Tiles[snapshot.CrateFromX, snapshot.CrateFromY] = snapshot.CrateFromTile;
                board.Tiles[snapshot.CrateToX, snapshot.CrateToY] = snapshot.CrateToTile;
            }

            player.Location = snapshot.PlayerLocation;
            player.Direction = snapshot.PlayerDirection;
            player.MovesCount = snapshot.MovesCount;
        }

        public UndoInfo PeekUndo()
        {
            MoveSnapshot snapshot = undoHistory.Peek();

            return new UndoInfo(
                snapshot.PlayerLocation,
                snapshot.CratePushed,
                new Point2D(snapshot.CrateToX, snapshot.CrateToY),
                new Point2D(snapshot.CrateFromX, snapshot.CrateFromY));
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
