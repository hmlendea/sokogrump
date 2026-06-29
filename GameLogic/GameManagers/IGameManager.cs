using System;
using System.Collections.Generic;

using NuciXNA.Primitives;

using SokoGrump.Models;

namespace SokoGrump.GameLogic.GameManagers
{
    public interface IGameManager : IGameLogicManager
    {
        bool Completed { get; }
        bool CanUndo { get; }
        int Level { get; }
        TimeSpan ElapsedTime { get; }

        Player GetPlayer();
        List<Point2D> GetTargets();
        Tile GetTile(int x, int y);
        IEnumerable<Tile> GetTiles();

        void SetPlayerDirection(MovementDirection direction);
        bool CanMove(MovementDirection direction);
        void MovePlayer(MovementDirection direction);
        void Undo();
        UndoInfo PeekUndo();
        void NewGame(int level);
        void Retry();
    }
}