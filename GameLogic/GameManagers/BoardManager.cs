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
        Dictionary<int, Tile> tiles;

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

        public Board GetBoard(int id) => new(boards[id.ToString()]);

        public Tile GetTile(int id) => new(tiles[id]);

        public IEnumerable<Tile> GetTiles() => tiles.Values;

        void LoadBoards() => boards = new BoardRepository(ApplicationPaths.LevelsDirectory).GetAll().ToDictionary(x => x.Id, x => x.ToDomainModel());

        void LoadTiles()
        {
            Tile terrainTile = new()
            {
                Id = 0,
                SpriteSheet = "SpriteSheets/brick",
                TileType = TileType.Walkable
            };
            Tile wallTile = new()
            {
                Id = 1,
                SpriteSheet = "SpriteSheets/wall",
                TileType = TileType.Solid
            };
            Tile boxTile = new()
            {
                Id = 2,
                SpriteSheet = "SpriteSheets/crate",
                TileType = TileType.Moveable
            };
            Tile targetTile = new()
            {
                Id = 3,
                SpriteSheet = "Tiles/tile3/0",
                TileType = TileType.Walkable
            };
            Tile completedTargetTile = new()
            {
                Id = 5,
                SpriteSheet = "Tiles/tile5/0",
                TileType = TileType.Moveable
            };
            Tile voidTile = new()
            {
                Id = 7,
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
