using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.GameLogic.Mapping;
using SokoGrump.Models;
using SokoGrump.UnitTests.Helpers;

namespace SokoGrump.UnitTests.GameLogic.Mapping
{
    [TestFixture]
    public class TileMappingExtensionsTests
    {
        // -------------------------------------------------------------------------
        // TileEntity.ToDomainModel
        // -------------------------------------------------------------------------

        [Test]
        public void GivenFloorTileEntity_WhenToDomainModel_ThenIdIsFloor()
        {
            TileEntity entity = BoardTestHelper.CreateTileEntity(TileId.Floor, "floor", "Walkable");

            Tile tile = entity.ToDomainModel();

            Assert.That(tile.Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenWallTileEntity_WhenToDomainModel_ThenIdIsWall()
        {
            TileEntity entity = BoardTestHelper.CreateTileEntity(TileId.Wall, "wall", "Solid");

            Tile tile = entity.ToDomainModel();

            Assert.That(tile.Id, Is.EqualTo(TileId.Wall));
        }

        [Test]
        public void GivenTileEntity_WhenToDomainModel_ThenSpriteSheetIsPreserved()
        {
            TileEntity entity = BoardTestHelper.CreateTileEntity(TileId.Floor, "SpriteSheets/brick", "Walkable");

            Tile tile = entity.ToDomainModel();

            Assert.That(tile.SpriteSheet, Is.EqualTo("SpriteSheets/brick"));
        }

        [Test]
        public void GivenWalkableTileEntity_WhenToDomainModel_ThenTileTypeIsWalkable()
        {
            TileEntity entity = BoardTestHelper.CreateTileEntity(TileId.Floor, "floor", "Walkable");

            Tile tile = entity.ToDomainModel();

            Assert.That(tile.TileType, Is.EqualTo(TileType.Walkable));
        }

        [Test]
        public void GivenSolidTileEntity_WhenToDomainModel_ThenTileTypeIsSolid()
        {
            TileEntity entity = BoardTestHelper.CreateTileEntity(TileId.Wall, "wall", "Solid");

            Tile tile = entity.ToDomainModel();

            Assert.That(tile.TileType, Is.EqualTo(TileType.Solid));
        }

        [Test]
        public void GivenMoveableTileEntity_WhenToDomainModel_ThenTileTypeIsMoveable()
        {
            TileEntity entity = BoardTestHelper.CreateTileEntity(TileId.CrateOnFloor, "crate", "Moveable");

            Tile tile = entity.ToDomainModel();

            Assert.That(tile.TileType, Is.EqualTo(TileType.Moveable));
        }

        // -------------------------------------------------------------------------
        // Tile.ToDataObject
        // -------------------------------------------------------------------------

        [Test]
        public void GivenFloorTile_WhenToDataObject_ThenIdIsZero()
        {
            Tile tile = BoardTestHelper.CreateFloorTile();

            TileEntity entity = tile.ToDataObject();

            Assert.That(entity.Id, Is.EqualTo((int)TileId.Floor));
        }

        [Test]
        public void GivenWallTile_WhenToDataObject_ThenIdIsOne()
        {
            Tile tile = BoardTestHelper.CreateWallTile();

            TileEntity entity = tile.ToDataObject();

            Assert.That(entity.Id, Is.EqualTo((int)TileId.Wall));
        }

        [Test]
        public void GivenTile_WhenToDataObject_ThenSpriteSheetIsPreserved()
        {
            Tile tile = new() { Id = TileId.Floor, SpriteSheet = "SpriteSheets/brick", TileType = TileType.Walkable };

            TileEntity entity = tile.ToDataObject();

            Assert.That(entity.SpriteSheet, Is.EqualTo("SpriteSheets/brick"));
        }

        [Test]
        public void GivenWalkableTile_WhenToDataObject_ThenTileTypeStringIsWalkable()
        {
            Tile tile = BoardTestHelper.CreateFloorTile();

            TileEntity entity = tile.ToDataObject();

            Assert.That(entity.TileType, Is.EqualTo("Walkable"));
        }

        [Test]
        public void GivenSolidTile_WhenToDataObject_ThenTileTypeStringIsSolid()
        {
            Tile tile = BoardTestHelper.CreateWallTile();

            TileEntity entity = tile.ToDataObject();

            Assert.That(entity.TileType, Is.EqualTo("Solid"));
        }

        [Test]
        public void GivenMoveableTile_WhenToDataObject_ThenTileTypeStringIsMoveable()
        {
            Tile tile = BoardTestHelper.CreateCrateTile();

            TileEntity entity = tile.ToDataObject();

            Assert.That(entity.TileType, Is.EqualTo("Moveable"));
        }

        // -------------------------------------------------------------------------
        // IEnumerable<TileEntity>.ToDomainModels
        // -------------------------------------------------------------------------

        [Test]
        public void GivenTileEntityCollection_WhenToDomainModels_ThenAllTilesAreMapped()
        {
            List<TileEntity> entities =
            [
                BoardTestHelper.CreateTileEntity(TileId.Floor, "floor", "Walkable"),
                BoardTestHelper.CreateTileEntity(TileId.Wall, "wall", "Solid"),
            ];

            List<Tile> tiles = entities.ToDomainModels().ToList();

            Assert.That(tiles.Count, Is.EqualTo(2));
            Assert.That(tiles[0].Id, Is.EqualTo(TileId.Floor));
            Assert.That(tiles[1].Id, Is.EqualTo(TileId.Wall));
        }

        // -------------------------------------------------------------------------
        // IEnumerable<Tile>.ToDataObjects
        // -------------------------------------------------------------------------

        [Test]
        public void GivenTileCollection_WhenToDataObjects_ThenAllEntitiesAreMapped()
        {
            List<Tile> tiles =
            [
                BoardTestHelper.CreateFloorTile(),
                BoardTestHelper.CreateWallTile(),
            ];

            List<TileEntity> entities = tiles.ToDataObjects().ToList();

            Assert.That(entities.Count, Is.EqualTo(2));
            Assert.That(entities[0].Id, Is.EqualTo((int)TileId.Floor));
            Assert.That(entities[1].Id, Is.EqualTo((int)TileId.Wall));
        }

        // -------------------------------------------------------------------------
        // TileEntity[,].ToDomainModels
        // -------------------------------------------------------------------------

        [Test]
        public void GivenTileEntityGrid_WhenToDomainModels_ThenDimensionsArePreserved()
        {
            TileEntity[,] entities = new TileEntity[3, 2];

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    entities[x, y] = BoardTestHelper.CreateTileEntity(TileId.Floor, "floor", "Walkable");
                }
            }

            Tile[,] tiles = entities.ToDomainModels();

            Assert.That(tiles.GetLength(0), Is.EqualTo(3));
            Assert.That(tiles.GetLength(1), Is.EqualTo(2));
        }

        [Test]
        public void GivenTileEntityGrid_WhenToDomainModels_ThenAllTilesAreMapped()
        {
            TileEntity[,] entities = new TileEntity[2, 2];
            entities[0, 0] = BoardTestHelper.CreateTileEntity(TileId.Floor, "floor", "Walkable");
            entities[1, 0] = BoardTestHelper.CreateTileEntity(TileId.Wall, "wall", "Solid");
            entities[0, 1] = BoardTestHelper.CreateTileEntity(TileId.CrateOnFloor, "crate", "Moveable");
            entities[1, 1] = BoardTestHelper.CreateTileEntity(TileId.Floor, "floor", "Walkable");

            Tile[,] tiles = entities.ToDomainModels();

            Assert.That(tiles[0, 0].Id, Is.EqualTo(TileId.Floor));
            Assert.That(tiles[1, 0].Id, Is.EqualTo(TileId.Wall));
            Assert.That(tiles[0, 1].Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        // -------------------------------------------------------------------------
        // Tile[,].ToDataObjects
        // -------------------------------------------------------------------------

        [Test]
        public void GivenTileGrid_WhenToDataObjects_ThenDimensionsArePreserved()
        {
            Tile[,] tiles = new Tile[3, 2];

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    tiles[x, y] = BoardTestHelper.CreateFloorTile();
                }
            }

            TileEntity[,] entities = tiles.ToDataObjects();

            Assert.That(entities.GetLength(0), Is.EqualTo(3));
            Assert.That(entities.GetLength(1), Is.EqualTo(2));
        }

        [Test]
        public void GivenTileGrid_WhenToDataObjects_ThenAllEntitiesAreMapped()
        {
            Tile[,] tiles = new Tile[2, 2];
            tiles[0, 0] = BoardTestHelper.CreateFloorTile();
            tiles[1, 0] = BoardTestHelper.CreateWallTile();
            tiles[0, 1] = BoardTestHelper.CreateCrateTile();
            tiles[1, 1] = BoardTestHelper.CreateFloorTile();

            TileEntity[,] entities = tiles.ToDataObjects();

            Assert.That(entities[0, 0].Id, Is.EqualTo((int)TileId.Floor));
            Assert.That(entities[1, 0].Id, Is.EqualTo((int)TileId.Wall));
            Assert.That(entities[0, 1].Id, Is.EqualTo((int)TileId.CrateOnFloor));
        }
    }
}
