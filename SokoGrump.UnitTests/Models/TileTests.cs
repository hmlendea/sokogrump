using NUnit.Framework;

using SokoGrump.Models;
using SokoGrump.UnitTests.Helpers;

namespace SokoGrump.UnitTests.Models
{
    [TestFixture]
    public sealed class TileTests
    {
        // -------------------------------------------------------------------------
        // Clone
        // -------------------------------------------------------------------------

        [Test]
        public void GivenTile_WhenClone_ThenIdIsPreserved()
        {
            Tile tile = BoardTestHelper.CreateWallTile();

            Tile clone = tile.Clone();

            Assert.That(clone.Id, Is.EqualTo(TileId.Wall));
        }

        [Test]
        public void GivenTile_WhenClone_ThenSpriteSheetIsPreserved()
        {
            Tile tile = new() { Id = TileId.Floor, SpriteSheet = "SpriteSheets/brick", TileType = TileType.Walkable };

            Tile clone = tile.Clone();

            Assert.That(clone.SpriteSheet, Is.EqualTo("SpriteSheets/brick"));
        }

        [Test]
        public void GivenTile_WhenClone_ThenTileTypeIsPreserved()
        {
            Tile tile = BoardTestHelper.CreateWallTile();

            Tile clone = tile.Clone();

            Assert.That(clone.TileType, Is.EqualTo(TileType.Solid));
        }

        [Test]
        public void GivenTileWithVariation_WhenClone_ThenVariationIsPreserved()
        {
            Tile tile = BoardTestHelper.CreateCrateTile(variation: 7);

            Tile clone = tile.Clone();

            Assert.That(clone.Variation, Is.EqualTo(7));
        }

        [Test]
        public void GivenTile_WhenClone_ThenCloneIsIndependentFromOriginal()
        {
            Tile tile = BoardTestHelper.CreateFloorTile();

            Tile clone = tile.Clone();
            clone.Id = TileId.Wall;

            Assert.That(tile.Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenTile_WhenClone_ThenCloneIsNotSameInstance()
        {
            Tile tile = BoardTestHelper.CreateFloorTile();

            Tile clone = tile.Clone();

            Assert.That(clone, Is.Not.SameAs(tile));
        }
    }
}
