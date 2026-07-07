using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciXNA.Primitives;

using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.GameLogic.GameManagers
{
    public class EditorManager : IEditorManager
    {
        readonly BoardManager boardManager;

        Board board;
        Player player;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngine"/> class.
        /// </summary>
        public EditorManager()
        {
            boardManager = new BoardManager();
        }

        public void LoadContent()
        {
            boardManager.LoadContent();
            board = boardManager.GetBoard(10);

            player = new Player();
        }

        public void UnloadContent()
        {
            boardManager.UnloadContent();
        }

        public void SaveContent()
        {
            string fileContents = string.Empty;

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    if (board.PlayerStartLocation.X == x &&
                        board.PlayerStartLocation.Y == y)
                    {
                        fileContents += 4;
                    }
                    else
                    {
                        fileContents += GetTile(x, y).Id;
                    }
                }

                fileContents += Environment.NewLine;
            }

            // TODO: Save to a user-inputted path
            string path = Path.Combine(ApplicationPaths.UserDataDirectory, "editor.lvl");

            File.WriteAllText(path, fileContents);
        }

        public void Update(double elapsedMiliseconds)
        {
            boardManager.Update(elapsedMiliseconds);

            player.Location = new Point2D(
                board.PlayerStartLocation.X,
                board.PlayerStartLocation.Y);
        }

        public List<Point2D> GetTargets()
        {
            return board.Targets;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public Tile GetTile(int x, int y)
        {
            return board.Tiles[x, y];
        }

        public void SetTile(int x, int y, int tileId)
        {
            TileId id = (TileId)tileId;

            if (id.Equals(TileId.EmptyTarget))
            {
                board.Tiles[x, y] = boardManager.GetTile(TileId.Floor);

                if (board.Targets.All(target => target.X != x || target.Y != y))
                {
                    board.Targets.Add(new Point2D(x, y));
                }
            }
            else if (id.Equals(TileId.PlayerOnFloor))
            {
                board.PlayerStartLocation = new Point2D(x, y);

                if (!board.Tiles[x, y].Id.Equals(TileId.Floor) &&
                    !board.Tiles[x, y].Id.Equals(TileId.EmptyTarget))
                {
                    board.Tiles[x, y] = boardManager.GetTile(TileId.Floor);
                }
            }
            else if (id.Equals(TileId.CrateOnTarget))
            {
                board.Tiles[x, y] = boardManager.GetTile(TileId.CrateOnFloor);

                if (board.Targets.All(target => target.X != x || target.Y != y))
                {
                    board.Targets.Add(new Point2D(x, y));
                }
            }
            else
            {
                board.Tiles[x, y] = boardManager.GetTile(id);
                board.Targets.RemoveAll(target => target.X == x && target.Y == y);
            }
        }

        public IEnumerable<Tile> GetTiles()
        {
            return boardManager.GetTiles();
        }
    }
}
