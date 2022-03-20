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

        public void Update(double elapsedMiliseconds)
        {

        }

        public Board GetBoard(int id)
        {
            Board loadedBoard = boards[id.ToString()];
            return new Board(loadedBoard);
        }

        public Tile GetTile(int id)
        {
            return tiles[id];
        }

        public IEnumerable<Tile> GetTiles()
        {
            return tiles.Values;
        }

        void LoadBoards()
        {
            BoardRepository repository = new BoardRepository(ApplicationPaths.LevelsDirectory);

            boards = repository.GetAll().ToDictionary(x => x.Id, x => x.ToDomainModel());
        }

        void LoadTiles()
        {
            Tile terrainTile = new Tile
            {
                Id = 0,
                SpriteSheet = "SpriteSheets/brick",
                TileType = TileType.Walkable
            };
            Tile wallTile = new Tile
            {
                Id = 1,
                SpriteSheet = "SpriteSheets/wall",
                TileType = TileType.Solid
            };
            Tile boxTile = new Tile
            {
                Id = 2,
                SpriteSheet = "SpriteSheets/crate",
                TileType = TileType.Moveable
            };
            Tile targetTile = new Tile
            {
                Id = 3,
                SpriteSheet = "Tiles/tile3/0",
                TileType = TileType.Walkable
            };
            Tile completedTargetTile = new Tile
            {
                Id = 5,
                SpriteSheet = "Tiles/tile5/0",
                TileType = TileType.Moveable
            };
            Tile voidTile = new Tile
            {
                Id = 7,
                SpriteSheet = "Tiles/tile7/0",
                TileType = TileType.Solid
            };

            tiles = new Dictionary<int, Tile>
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
