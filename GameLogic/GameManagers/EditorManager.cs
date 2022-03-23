using System.Collections.Generic;

using NuciXNA.Primitives;

using SokoGrump.Models;

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

        public void Update(double elapsedMiliseconds)
        {
            boardManager.Update(elapsedMiliseconds);
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

        public IEnumerable<Tile> GetTiles()
        {
            return boardManager.GetTiles();
        }
    }
}
