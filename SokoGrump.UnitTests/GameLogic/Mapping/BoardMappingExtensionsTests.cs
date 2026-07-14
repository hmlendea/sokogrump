using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NuciXNA.Primitives;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.GameLogic.Mapping;
using SokoGrump.Models;
using SokoGrump.UnitTests.Helpers;

namespace SokoGrump.UnitTests.GameLogic.Mapping
{
    [TestFixture]
    public class BoardMappingExtensionsTests
    {
        // -------------------------------------------------------------------------
        // BoardEntity.ToDomainModel
        // -------------------------------------------------------------------------

        [Test]
        public void GivenBoardEntity_WhenToDomainModel_ThenIdIsPreserved()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity();
            entity.Id = "42";

            Board board = entity.ToDomainModel();

            Assert.That(board.Id, Is.EqualTo("42"));
        }

        [Test]
        public void GivenBoardEntity_WhenToDomainModel_ThenPlayerStartLocationXIsPreserved()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(playerX: 3, playerY: 5);

            Board board = entity.ToDomainModel();

            Assert.That(board.PlayerStartLocation.X, Is.EqualTo(3));
        }

        [Test]
        public void GivenBoardEntity_WhenToDomainModel_ThenPlayerStartLocationYIsPreserved()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(playerX: 3, playerY: 5);

            Board board = entity.ToDomainModel();

            Assert.That(board.PlayerStartLocation.Y, Is.EqualTo(5));
        }

        [Test]
        public void GivenBoardEntityWithNoTargetTiles_WhenToDomainModel_ThenTargetsListIsEmpty()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity();
            // All tiles are Floor - no targets

            Board board = entity.ToDomainModel();

            Assert.That(board.Targets, Is.Empty);
        }

        [Test]
        public void GivenBoardEntityWithEmptyTargetTile_WhenToDomainModel_ThenTargetIsDetected()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(width: 4, height: 4);
            entity.Tiles[2, 1] = BoardTestHelper.CreateTileEntity(TileId.EmptyTarget, "target", "Walkable");

            Board board = entity.ToDomainModel();

            Assert.That(board.Targets.Any(target => target.X == 2 && target.Y == 1), Is.True);
        }

        [Test]
        public void GivenBoardEntityWithCrateOnTargetTile_WhenToDomainModel_ThenTargetIsDetected()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(width: 4, height: 4);
            entity.Tiles[1, 3] = BoardTestHelper.CreateTileEntity(TileId.CrateOnTarget, "crate_target", "Moveable");

            Board board = entity.ToDomainModel();

            Assert.That(board.Targets.Any(target => target.X == 1 && target.Y == 3), Is.True);
        }

        [Test]
        public void GivenBoardEntityWithMultipleTargets_WhenToDomainModel_ThenAllTargetsAreDetected()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(width: 4, height: 4);
            entity.Tiles[0, 0] = BoardTestHelper.CreateTileEntity(TileId.EmptyTarget, "target", "Walkable");
            entity.Tiles[2, 2] = BoardTestHelper.CreateTileEntity(TileId.CrateOnTarget, "crate_target", "Moveable");
            entity.Tiles[3, 1] = BoardTestHelper.CreateTileEntity(TileId.EmptyTarget, "target", "Walkable");

            Board board = entity.ToDomainModel();

            Assert.That(board.Targets.Count, Is.EqualTo(3));
        }

        [Test]
        public void GivenBoardEntityWithEmptyTarget_WhenToDomainModel_ThenTargetCoordinatesAreCorrect()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(width: 4, height: 4);
            entity.Tiles[2, 3] = BoardTestHelper.CreateTileEntity(TileId.EmptyTarget, "target", "Walkable");

            Board board = entity.ToDomainModel();

            Point2D target = board.Targets.Single();
            Assert.That(target.X, Is.EqualTo(2));
            Assert.That(target.Y, Is.EqualTo(3));
        }

        [Test]
        public void GivenBoardEntity_WhenToDomainModel_ThenTilesAreMapped()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(width: 2, height: 2);
            entity.Tiles[1, 0] = BoardTestHelper.CreateTileEntity(TileId.Wall, "wall", "Solid");

            Board board = entity.ToDomainModel();

            Assert.That(board.Tiles[1, 0].Id, Is.EqualTo(TileId.Wall));
        }

        // -------------------------------------------------------------------------
        // Board.ToDataObject
        // -------------------------------------------------------------------------

        [Test]
        public void GivenBoard_WhenToDataObject_ThenIdIsPreserved()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity();
            entity.Id = "99";
            Board board = entity.ToDomainModel();

            BoardEntity result = board.ToDataObject();

            Assert.That(result.Id, Is.EqualTo("99"));
        }

        [Test]
        public void GivenBoard_WhenToDataObject_ThenPlayerStartLocationXIsPreserved()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(playerX: 7, playerY: 2);
            Board board = entity.ToDomainModel();

            BoardEntity result = board.ToDataObject();

            Assert.That(result.PlayerStartLocationX, Is.EqualTo(7));
        }

        [Test]
        public void GivenBoard_WhenToDataObject_ThenPlayerStartLocationYIsPreserved()
        {
            BoardEntity entity = BoardTestHelper.CreateBoardEntity(playerX: 7, playerY: 2);
            Board board = entity.ToDomainModel();

            BoardEntity result = board.ToDataObject();

            Assert.That(result.PlayerStartLocationY, Is.EqualTo(2));
        }

        // -------------------------------------------------------------------------
        // IEnumerable<BoardEntity>.ToDomainModels
        // -------------------------------------------------------------------------

        [Test]
        public void GivenBoardEntityCollection_WhenToDomainModels_ThenAllBoardsAreMapped()
        {
            BoardEntity entity1 = BoardTestHelper.CreateBoardEntity();
            entity1.Id = "1";
            BoardEntity entity2 = BoardTestHelper.CreateBoardEntity();
            entity2.Id = "2";

            List<Board> boards = new List<BoardEntity> { entity1, entity2 }.ToDomainModels().ToList();

            Assert.That(boards.Count, Is.EqualTo(2));
            Assert.That(boards[0].Id, Is.EqualTo("1"));
            Assert.That(boards[1].Id, Is.EqualTo("2"));
        }

        // -------------------------------------------------------------------------
        // IEnumerable<Board>.ToDataObjects
        // -------------------------------------------------------------------------

        [Test]
        public void GivenBoardCollection_WhenToDataObjects_ThenAllEntitiesAreMapped()
        {
            BoardEntity entity1 = BoardTestHelper.CreateBoardEntity();
            entity1.Id = "A";
            BoardEntity entity2 = BoardTestHelper.CreateBoardEntity();
            entity2.Id = "B";

            List<BoardEntity> entities = new List<BoardEntity> { entity1, entity2 }
                .ToDomainModels()
                .ToDataObjects()
                .ToList();

            Assert.That(entities.Count, Is.EqualTo(2));
            Assert.That(entities[0].Id, Is.EqualTo("A"));
            Assert.That(entities[1].Id, Is.EqualTo("B"));
        }
    }
}
