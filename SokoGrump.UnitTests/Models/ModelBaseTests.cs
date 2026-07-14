using NUnit.Framework;

using SokoGrump.Models;

namespace SokoGrump.UnitTests.Models
{
    [TestFixture]
    public class ModelBaseTests
    {
        // -------------------------------------------------------------------------
        // Equals(ModelBase)
        // -------------------------------------------------------------------------

        [Test]
        public void GivenSameIdNameDescription_WhenEquals_ThenReturnsTrue()
        {
            Board first = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };
            Board second = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(first.Equals(second), Is.True);
        }

        [Test]
        public void GivenDifferentId_WhenEquals_ThenReturnsFalse()
        {
            Board first = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };
            Board second = new() { Id = "bravo", Name = "TestBoard", Description = "A test board." };

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void GivenDifferentName_WhenEquals_ThenReturnsFalse()
        {
            Board first = new() { Id = "alpha", Name = "FirstBoard", Description = "A test board." };
            Board second = new() { Id = "alpha", Name = "SecondBoard", Description = "A test board." };

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void GivenDifferentDescription_WhenEquals_ThenReturnsFalse()
        {
            Board first = new()
            {
                Id = "alpha",
                Name = "TestBoard",
                Description = "Description one."
            };
            Board second = new()
            {
                Id = "alpha",
                Name = "TestBoard",
                Description = "Description two."
            };

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void GivenNullOther_WhenEquals_ThenReturnsFalse()
        {
            Board board = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(board.Equals((ModelBase)null), Is.False);
        }

        [Test]
        public void GivenSameReference_WhenEquals_ThenReturnsTrue()
        {
            Board board = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(board.Equals(board), Is.True);
        }

        // -------------------------------------------------------------------------
        // Equals(object)
        // -------------------------------------------------------------------------

        [Test]
        public void GivenNullObject_WhenEqualsObject_ThenReturnsFalse()
        {
            Board board = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(board.Equals((object)null), Is.False);
        }

        [Test]
        public void GivenSameReferenceAsObject_WhenEqualsObject_ThenReturnsTrue()
        {
            Board board = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(board.Equals((object)board), Is.True);
        }

        [Test]
        public void GivenDifferentTypeObject_WhenEqualsObject_ThenReturnsFalse()
        {
            Board board = new()
            {
                Id = "alpha",
                Name = "TestBoard",
                Description = "A test board."
            };
            Player player = new()
            {
                Id = "alpha",
                Name = "TestBoard",
                Description = "A test board."
            };

            Assert.That(board.Equals((object)player), Is.False);
        }

        [Test]
        public void GivenSameValuesAsObject_WhenEqualsObject_ThenReturnsTrue()
        {
            Board first = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };
            Board second = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(first.Equals((object)second), Is.True);
        }

        // -------------------------------------------------------------------------
        // GetHashCode
        // -------------------------------------------------------------------------

        [Test]
        public void GivenSameValues_WhenGetHashCode_ThenHashCodesAreEqual()
        {
            Board first = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };
            Board second = new() { Id = "alpha", Name = "TestBoard", Description = "A test board." };

            Assert.That(first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
        }

        [Test]
        public void GivenNullIdAndName_WhenGetHashCode_ThenDoesNotThrow()
        {
            Board board = new() { Description = "A test board." };

            Assert.That(() => board.GetHashCode(), Throws.Nothing);
        }
    }
}
