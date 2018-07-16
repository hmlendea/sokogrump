using System.Collections.Generic;

using NuciXNA.Primitives;

using SokoGrump.Models;

namespace SokoGrump.GameLogic.GameManagers
{
    public interface IGameManager : IGameLogicManager
    {
        bool Completed { get; }
        int Level { get; }

        Player GetPlayer();
        List<Point2D> GetTargets();
        Tile GetTile(int x, int y);
        IEnumerable<Tile> GetTiles();

        void MovePlayer(MovementDirection direction);
        void NewGame(int level);
        void Retry();
    }
}