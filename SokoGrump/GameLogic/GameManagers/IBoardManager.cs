using System.Collections.Generic;
using SokoGrump.Models;

namespace SokoGrump.GameLogic.GameManagers
{
    public interface IBoardManager : IGameLogicManager
    {
        Board GetBoard(int id);
        Tile GetTile(int id);
        IEnumerable<Tile> GetTiles();
    }
}