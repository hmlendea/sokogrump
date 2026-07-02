using NUnit.Framework;
using NuciXNA.Primitives;

using SokoGrump.Models;
using SokoGrump.Settings;
using SokoGrump.UnitTests.Helpers;

namespace SokoGrump.UnitTests.Models
{
    [TestFixture]
    public class BoardTests
    {
        // -------------------------------------------------------------------------
        // Clone
        // -------------------------------------------------------------------------

        [Test]
        public void GivenBoard_WhenClone_ThenPlayerStartLocationIsPreserved()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 5, playerY: 6);

            Board clone = board.Clone();

            Assert.That(clone.PlayerStartLocation.X, Is.EqualTo(5));
            Assert.That(clone.PlayerStartLocation.Y, Is.EqualTo(6));
        }

        [Test]
        public void GivenBoardWithTargets_WhenClone_ThenTargetsArePreserved()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Targets.Add(new Point2D(2, 3));
            board.Targets.Add(new Point2D(7, 9));

            Board clone = board.Clone();

            Assert.That(clone.Targets.Count, Is.EqualTo(2));
        }

        [Test]
        public void GivenBoardWithTargets_WhenClone_ThenTargetCoordinatesArePreserved()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Targets.Add(new Point2D(2, 3));

            Board clone = board.Clone();

            Assert.That(clone.Targets[0].X, Is.EqualTo(2));
            Assert.That(clone.Targets[0].Y, Is.EqualTo(3));
        }

        [Test]
        public void GivenBoard_WhenClone_ThenTilesArePreserved()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Tiles[3, 5] = BoardTestHelper.CreateWallTile();

            Board clone = board.Clone();

            Assert.That(clone.Tiles[3, 5].Id, Is.EqualTo(TileId.Wall));
        }

        [Test]
        public void GivenBoard_WhenClone_ThenTileDimensionsArePreserved()
        {
            Board board = BoardTestHelper.CreateBoard();

            Board clone = board.Clone();

            Assert.That(clone.Tiles.GetLength(0), Is.EqualTo(GameDefines.BoardWidth));
            Assert.That(clone.Tiles.GetLength(1), Is.EqualTo(GameDefines.BoardHeight));
        }

        [Test]
        public void GivenBoard_WhenClone_ThenModifyingCloneTilesDoesNotAffectOriginal()
        {
            Board board = BoardTestHelper.CreateBoard();

            Board clone = board.Clone();
            clone.Tiles[0, 0] = BoardTestHelper.CreateWallTile();

            Assert.That(board.Tiles[0, 0].Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenBoard_WhenClone_ThenModifyingCloneTargetsDoesNotAffectOriginal()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Targets.Add(new Point2D(1, 1));

            Board clone = board.Clone();
            clone.Targets.Add(new Point2D(2, 2));

            Assert.That(board.Targets.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenBoard_WhenClone_ThenCloneIsNotSameInstance()
        {
            Board board = BoardTestHelper.CreateBoard();

            Board clone = board.Clone();

            Assert.That(clone, Is.Not.SameAs(board));
        }
    }
}
