using NuciXNA.Primitives;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.UnitTests.Helpers
{
    internal static class BoardTestHelper
    {
        internal static Board CreateBoard(int playerX = 4, int playerY = 4)
        {
            Board board = new()
            {
                PlayerStartLocation = new Point2D(playerX, playerY),
                Tiles = new Tile[GameDefines.BoardWidth, GameDefines.BoardHeight]
            };

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    board.Tiles[x, y] = CreateFloorTile();
                }
            }

            return board;
        }

        internal static Tile CreateFloorTile() => new()
        {
            Id = TileId.Floor,
            SpriteSheet = "floor",
            TileType = TileType.Walkable
        };

        internal static Tile CreateWallTile() => new()
        {
            Id = TileId.Wall,
            SpriteSheet = "wall",
            TileType = TileType.Solid
        };

        internal static Tile CreateCrateTile(int variation = 0) => new()
        {
            Id = TileId.CrateOnFloor,
            SpriteSheet = "crate",
            TileType = TileType.Moveable,
            Variation = variation
        };

        internal static TileEntity CreateTileEntity(TileId id, string spriteSheet, string tileType) => new()
        {
            Id = (int)id,
            SpriteSheet = spriteSheet,
            TileType = tileType
        };

        internal static BoardEntity CreateBoardEntity(int width = 4, int height = 4, int playerX = 1, int playerY = 1)
        {
            BoardEntity entity = new()
            {
                Id = "test",
                PlayerStartLocationX = playerX,
                PlayerStartLocationY = playerY,
                Tiles = new TileEntity[width, height]
            };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    entity.Tiles[x, y] = CreateTileEntity(TileId.Floor, "floor", "Walkable");
                }
            }

            return entity;
        }
    }
}
