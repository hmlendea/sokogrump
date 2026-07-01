using System.Collections.Generic;
using System.Linq;

using SokoGrump.DataAccess.Repositories;
using SokoGrump.GameLogic.Mapping;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.GameLogic.GameManagers
{
    public class BoardManager : IBoardManager
    {
        Dictionary<string, Board> boards;
        Dictionary<TileId, Tile> tiles;

        public void LoadContent()
        {
            LoadBoards();
            LoadTiles();
        }

        public void UnloadContent()
        {
            boards.Clear();
            tiles.Clear();
        }

        public void Update(double elapsedMiliseconds) { }

        public Board GetBoard(int id) => boards[id.ToString()].Clone();

        public Tile GetTile(TileId id) => new(tiles[id]);

        public IEnumerable<Tile> GetTiles() => tiles.Values;

        void LoadBoards() => boards = new BoardRepository(ApplicationPaths.LevelsDirectory).GetAll().ToDictionary(x => x.Id, x => x.ToDomainModel());

        void LoadTiles()
        {
            Tile terrainTile = new()
            {
                Id = TileId.Ground,
                SpriteSheet = "SpriteSheets/brick",
                TileType = TileType.Walkable
            };
            Tile wallTile = new()
            {
                Id = TileId.Wall,
                SpriteSheet = "SpriteSheets/wall",
                TileType = TileType.Solid
            };
            Tile boxTile = new()
            {
                Id = TileId.CrateOnGround,
                SpriteSheet = "SpriteSheets/crate",
                TileType = TileType.Moveable
            };
            Tile targetTile = new()
            {
                Id = TileId.EmptyTarget,
                SpriteSheet = "Tiles/tile3/0",
                TileType = TileType.Walkable
            };
            Tile completedTargetTile = new()
            {
                Id = TileId.CrateOnTarget,
                SpriteSheet = "Tiles/tile5/0",
                TileType = TileType.Moveable
            };
            Tile voidTile = new()
            {
                Id = TileId.Void,
                SpriteSheet = "Tiles/tile7/0",
                TileType = TileType.Solid
            };

            tiles = new()
            {
                { terrainTile.Id, terrainTile },
                { wallTile.Id, wallTile },
                { boxTile.Id, boxTile },
                { targetTile.Id, targetTile },
                { completedTargetTile.Id, completedTargetTile },
                { voidTile.Id, voidTile }
            };
        }
    }
}
