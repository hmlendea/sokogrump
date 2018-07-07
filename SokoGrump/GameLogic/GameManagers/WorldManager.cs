using System.Collections.Generic;

using SokoGrump.Models;

namespace SokoGrump.GameLogic.GameManagers
{
    public class WorldManager
    {
        Dictionary<int, Tile> tiles;

        public void LoadContent()
        {
            tiles = new Dictionary<int, Tile>();

            Tile terrainTile = new Tile
            {
                Id = 0,
                SpriteSheet = "Tiles/tile0/0",
                TileType = TileType.Walkable
            };
            Tile wallTile = new Tile
            {
                Id = 1,
                SpriteSheet = "Tiles/tile1/0",
                TileType = TileType.Solid
            };
            Tile boxTile = new Tile
            {
                Id = 2,
                SpriteSheet = "Tiles/tile2/0",
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

            tiles.Add(terrainTile.Id, terrainTile);
            tiles.Add(wallTile.Id, wallTile);
            tiles.Add(boxTile.Id, boxTile);
            tiles.Add(targetTile.Id, targetTile);
            tiles.Add(completedTargetTile.Id, completedTargetTile);
            tiles.Add(voidTile.Id, voidTile);
        }

        public void UnloadContent()
        {
            tiles.Clear();
        }

        public Tile GetTile(int id)
        {
            return tiles[id];
        }

        public IEnumerable<Tile> GetTiles()
        {
            return tiles.Values;
        }
    }
}
