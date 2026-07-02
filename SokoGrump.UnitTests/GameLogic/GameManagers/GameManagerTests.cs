using System;
using System.Collections.Generic;

using Moq;
using NUnit.Framework;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;
using SokoGrump.UnitTests.Helpers;

namespace SokoGrump.UnitTests.GameLogic.GameManagers
{
    [TestFixture]
    public class GameManagerTests
    {
        Mock<IBoardManager> _boardManagerMock;
        GameManager _gameManager;

        [SetUp]
        public void SetUp()
        {
            _boardManagerMock = new Mock<IBoardManager>();
            _boardManagerMock.Setup(m => m.GetTile(TileId.Floor)).Returns(() => BoardTestHelper.CreateFloorTile());
            _boardManagerMock.Setup(m => m.GetTile(TileId.CrateOnFloor)).Returns(() => BoardTestHelper.CreateCrateTile());
            _gameManager = new GameManager(_boardManagerMock.Object);
        }

        void StartGameWithBoard(Board board, int level = 1)
        {
            _boardManagerMock.Setup(m => m.GetBoard(level)).Returns(board);
            _gameManager.NewGame(level);
        }

        // -------------------------------------------------------------------------
        // NewGame
        // -------------------------------------------------------------------------

        [Test]
        public void GivenLevel3_WhenNewGame_ThenLevelIsSet()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board, level: 3);

            Assert.That(_gameManager.Level, Is.EqualTo(3));
        }

        [Test]
        public void GivenElapsedTime_WhenNewGame_ThenElapsedTimeIsReset()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.Update(500);

            StartGameWithBoard(board);

            Assert.That(_gameManager.ElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void GivenEmptyTargetTile_WhenNewGame_ThenTileIsReplacedWithFloor()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Tiles[2, 2] = new Tile { Id = TileId.EmptyTarget, TileType = TileType.Walkable, SpriteSheet = "target" };
            StartGameWithBoard(board);

            Assert.That(_gameManager.GetTile(2, 2).Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenCrateOnTargetTile_WhenNewGame_ThenTileIsReplacedWithCrateOnFloor()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Tiles[2, 2] = new Tile { Id = TileId.CrateOnTarget, TileType = TileType.Moveable, SpriteSheet = "crate_target" };
            StartGameWithBoard(board);

            Assert.That(_gameManager.GetTile(2, 2).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenPreviousMoveHistory_WhenNewGame_ThenUndoHistoryIsCleared()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            StartGameWithBoard(board);

            Assert.That(_gameManager.CanUndo, Is.False);
        }

        [Test]
        public void GivenBoard_WhenNewGame_ThenPlayerStartsAtBoardStartLocation()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 6, playerY: 7);
            StartGameWithBoard(board);

            Player player = _gameManager.GetPlayer();

            Assert.That(player.Location.X, Is.EqualTo(6));
            Assert.That(player.Location.Y, Is.EqualTo(7));
        }

        // -------------------------------------------------------------------------
        // Retry
        // -------------------------------------------------------------------------

        [Test]
        public void GivenLevel5_WhenRetry_ThenSameLevelIsLoaded()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board, level: 5);

            _gameManager.Retry();

            Assert.That(_gameManager.Level, Is.EqualTo(5));
        }

        [Test]
        public void GivenMovesMade_WhenRetry_ThenUndoHistoryIsCleared()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board, level: 1);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Retry();

            Assert.That(_gameManager.CanUndo, Is.False);
        }

        // -------------------------------------------------------------------------
        // SetPlayerDirection
        // -------------------------------------------------------------------------

        [Test]
        public void GivenSouthDirection_WhenSetPlayerDirectionNorth_ThenDirectionIsNorth()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);

            _gameManager.SetPlayerDirection(MovementDirection.North);

            Assert.That(_gameManager.GetPlayer().Direction, Is.EqualTo(MovementDirection.North));
        }

        // -------------------------------------------------------------------------
        // CanMove
        // -------------------------------------------------------------------------

        [Test]
        public void GivenWalkableTileNorth_WhenCanMoveNorth_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.North), Is.True);
        }

        [Test]
        public void GivenWalkableTileSouth_WhenCanMoveSouth_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.South), Is.True);
        }

        [Test]
        public void GivenWalkableTileWest_WhenCanMoveWest_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.West), Is.True);
        }

        [Test]
        public void GivenWalkableTileEast_WhenCanMoveEast_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.East), Is.True);
        }

        [Test]
        public void GivenSolidTileNorth_WhenCanMoveNorth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.North), Is.False);
        }

        [Test]
        public void GivenSolidTileSouth_WhenCanMoveSouth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.South), Is.False);
        }

        [Test]
        public void GivenSolidTileWest_WhenCanMoveWest_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.West), Is.False);
        }

        [Test]
        public void GivenSolidTileEast_WhenCanMoveEast_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.East), Is.False);
        }

        [Test]
        public void GivenNorthBoardEdge_WhenCanMoveNorth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 0);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.North), Is.False);
        }

        [Test]
        public void GivenSouthBoardEdge_WhenCanMoveSouth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 13);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.South), Is.False);
        }

        [Test]
        public void GivenWestBoardEdge_WhenCanMoveWest_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 0, playerY: 4);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.West), Is.False);
        }

        [Test]
        public void GivenEastBoardEdge_WhenCanMoveEast_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 15, playerY: 4);
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.East), Is.False);
        }

        [Test]
        public void GivenCrateWithFloorBehindNorth_WhenCanMoveNorth_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            // board.Tiles[4, 2] is already Floor
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.North), Is.True);
        }

        [Test]
        public void GivenCrateWithWallBehindNorth_WhenCanMoveNorth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            board.Tiles[4, 2] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.North), Is.False);
        }

        [Test]
        public void GivenCrateAndPlayerTooCloseToNorthEdge_WhenCanMoveNorth_ThenReturnsFalse()
        {
            // Player at Y=1, crate at Y=0. dest2Y would be -1.
            // CanPushCrate: dirY=-1, player.Y=1 < 2 → dest2InBounds=false → false
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 1);
            board.Tiles[4, 0] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.North), Is.False);
        }

        // -------------------------------------------------------------------------
        // MovePlayer
        // -------------------------------------------------------------------------

        [Test]
        public void GivenWalkableTileNorth_WhenMovePlayer_ThenPlayerMovesNorth()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(3));
        }

        [Test]
        public void GivenWalkableTileSouth_WhenMovePlayer_ThenPlayerMovesSouth()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.South);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.Y, Is.EqualTo(5));
        }

        [Test]
        public void GivenWalkableTileWest_WhenMovePlayer_ThenPlayerMovesWest()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.West);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(3));
        }

        [Test]
        public void GivenWalkableTileEast_WhenMovePlayer_ThenPlayerMovesEast()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.East);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(5));
        }

        [Test]
        public void GivenWalkableTile_WhenMovePlayer_ThenMovesCountIsIncremented()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenWalkableTile_WhenMovePlayer_ThenDirectionIsUpdated()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.GetPlayer().Direction, Is.EqualTo(MovementDirection.North));
        }

        [Test]
        public void GivenSolidTileAhead_WhenMovePlayer_ThenPlayerDoesNotMove()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenSolidTileAhead_WhenMovePlayer_ThenMovesCountIsUnchanged()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(0));
        }

        [Test]
        public void GivenCrateWithFloorBehind_WhenMovePlayer_ThenPlayerMovesToCrateOriginalPosition()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(3));
        }

        [Test]
        public void GivenCrateWithFloorBehind_WhenMovePlayer_ThenCrateMovesForward()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.GetTile(4, 2).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCrateWithFloorBehind_WhenMovePlayer_ThenOriginalCratePositionBecomesFloor()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.GetTile(4, 3).Id, Is.EqualTo(TileId.Floor));
        }

        // -------------------------------------------------------------------------
        // CanUndo
        // -------------------------------------------------------------------------

        [Test]
        public void GivenNoMoves_WhenCheckCanUndo_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanUndo, Is.False);
        }

        [Test]
        public void GivenPlayerMoved_WhenCheckCanUndo_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.CanUndo, Is.True);
        }

        [Test]
        public void GivenPlayerMovedThenUndone_WhenCheckCanUndo_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);
            _gameManager.Undo();

            Assert.That(_gameManager.CanUndo, Is.False);
        }

        // -------------------------------------------------------------------------
        // Undo
        // -------------------------------------------------------------------------

        [Test]
        public void GivenEmptyHistory_WhenUndo_ThenPlayerLocationIsUnchanged()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);

            _gameManager.Undo();

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenPlayerMoved_WhenUndo_ThenPlayerReturnsToOriginalLocation()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Undo();

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenPlayerMoved_WhenUndo_ThenMovesCountIsReverted()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Undo();

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(0));
        }

        [Test]
        public void GivenPlayerMovedNorth_WhenUndo_ThenDirectionIsReverted()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            MovementDirection originalDirection = _gameManager.GetPlayer().Direction;
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Undo();

            Assert.That(_gameManager.GetPlayer().Direction, Is.EqualTo(originalDirection));
        }

        [Test]
        public void GivenCratePushedNorth_WhenUndo_ThenCrateReturnsToOriginalPosition()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Undo();

            Assert.That(_gameManager.GetTile(4, 3).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCratePushedNorth_WhenUndo_ThenCrateForwardPositionBecomesFloor()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Undo();

            Assert.That(_gameManager.GetTile(4, 2).Id, Is.EqualTo(TileId.Floor));
        }

        // -------------------------------------------------------------------------
        // PeekUndo
        // -------------------------------------------------------------------------

        [Test]
        public void GivenPlayerMovedNorth_WhenPeekUndo_ThenPlayerTargetIsOriginalLocation()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.PlayerTarget.X, Is.EqualTo(4));
            Assert.That(undoInfo.PlayerTarget.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenWalkMoveWithoutCrate_WhenPeekUndo_ThenCratePushedIsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CratePushed, Is.False);
        }

        [Test]
        public void GivenCratePushedNorth_WhenPeekUndo_ThenCratePushedIsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CratePushed, Is.True);
        }

        [Test]
        public void GivenCratePushedNorth_WhenPeekUndo_ThenCrateAnimStartIsWhereGrateEnded()
        {
            // Crate pushed from (4,3) to (4,2). AnimStart = where crate is now = (4,2).
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CrateAnimStart.X, Is.EqualTo(4));
            Assert.That(undoInfo.CrateAnimStart.Y, Is.EqualTo(2));
        }

        [Test]
        public void GivenCratePushedNorth_WhenPeekUndo_ThenCrateAnimEndIsWhereCrateCameFrom()
        {
            // Crate pushed from (4,3) to (4,2). AnimEnd = where crate came from = (4,3).
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CrateAnimEnd.X, Is.EqualTo(4));
            Assert.That(undoInfo.CrateAnimEnd.Y, Is.EqualTo(3));
        }

        // -------------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------------

        [Test]
        public void GivenNoTargets_WhenUpdate_ThenCompletedIsTrue()
        {
            Board board = BoardTestHelper.CreateBoard();
            // board.Targets is empty by default
            StartGameWithBoard(board);

            _gameManager.Update(0);

            Assert.That(_gameManager.Completed, Is.True);
        }

        [Test]
        public void GivenAllTargetsCoveredByCrates_WhenUpdate_ThenCompletedIsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Targets.Add(new Point2D(2, 2));
            board.Tiles[2, 2] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.Update(0);

            Assert.That(_gameManager.Completed, Is.True);
        }

        [Test]
        public void GivenTargetNotCoveredByCrate_WhenUpdate_ThenCompletedIsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Targets.Add(new Point2D(2, 2));
            // Tiles[2, 2] is Floor (not CrateOnFloor)
            StartGameWithBoard(board);

            _gameManager.Update(0);

            Assert.That(_gameManager.Completed, Is.False);
        }

        [Test]
        public void GivenNotCompleted_WhenUpdate_ThenElapsedTimeIncreases()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Targets.Add(new Point2D(2, 2));
            StartGameWithBoard(board);

            _gameManager.Update(500);

            Assert.That(_gameManager.ElapsedTime, Is.EqualTo(TimeSpan.FromMilliseconds(500)));
        }

        [Test]
        public void GivenCompleted_WhenUpdate_ThenElapsedTimeDoesNotIncrease()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Targets.Add(new Point2D(2, 2));
            board.Tiles[2, 2] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.Update(500);

            Assert.That(_gameManager.ElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void GivenMultipleUpdates_WhenNotCompleted_ThenElapsedTimeAccumulates()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Targets.Add(new Point2D(2, 2));
            StartGameWithBoard(board);

            _gameManager.Update(200);
            _gameManager.Update(300);

            Assert.That(_gameManager.ElapsedTime, Is.EqualTo(TimeSpan.FromMilliseconds(500)));
        }
    }
}
