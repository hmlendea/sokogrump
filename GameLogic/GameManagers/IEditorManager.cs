using System.Collections.Generic;

using NuciXNA.Primitives;

using SokoGrump.Models;

namespace SokoGrump.GameLogic.GameManagers
{
    public interface IEditorManager : IGameLogicManager
    {
        Player GetPlayer();
        List<Point2D> GetTargets();
        Tile GetTile(int x, int y);
        IEnumerable<Tile> GetTiles();
    }
}