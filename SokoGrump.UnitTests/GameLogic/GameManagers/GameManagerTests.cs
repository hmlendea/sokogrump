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

        [Test]
        public void GivenBoard_WhenNewGame_ThenPlayerDirectionIsSouth()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);

            Assert.That(
                _gameManager.GetPlayer().Direction,
                Is.EqualTo(MovementDirection.South));
        }

        [Test]
        public void GivenBoard_WhenNewGame_ThenPlayerMovesCountIsZero()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(0));
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

        [Test]
        public void GivenMoveMade_WhenRetry_ThenPlayerLocationIsReset()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 6, playerY: 7);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Retry();

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(6));
            Assert.That(player.Location.Y, Is.EqualTo(7));
        }

        [Test]
        public void GivenElapsedTime_WhenRetry_ThenElapsedTimeIsReset()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Targets.Add(new Point2D(2, 2));
            StartGameWithBoard(board);
            _gameManager.Update(500);

            _gameManager.Retry();

            Assert.That(_gameManager.ElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void GivenMoveMade_WhenRetry_ThenMovesCountIsReset()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);

            _gameManager.Retry();

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(0));
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

        [Test]
        public void GivenNorthDirection_WhenSetPlayerDirectionSouth_ThenDirectionIsSouth()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.SetPlayerDirection(MovementDirection.North);

            _gameManager.SetPlayerDirection(MovementDirection.South);

            Assert.That(
                _gameManager.GetPlayer().Direction,
                Is.EqualTo(MovementDirection.South));
        }

        [Test]
        public void GivenNorthDirection_WhenSetPlayerDirectionWest_ThenDirectionIsWest()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.SetPlayerDirection(MovementDirection.North);

            _gameManager.SetPlayerDirection(MovementDirection.West);

            Assert.That(
                _gameManager.GetPlayer().Direction,
                Is.EqualTo(MovementDirection.West));
        }

        [Test]
        public void GivenNorthDirection_WhenSetPlayerDirectionEast_ThenDirectionIsEast()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);
            _gameManager.SetPlayerDirection(MovementDirection.North);

            _gameManager.SetPlayerDirection(MovementDirection.East);

            Assert.That(
                _gameManager.GetPlayer().Direction,
                Is.EqualTo(MovementDirection.East));
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

        [Test]
        public void GivenCrateWithFloorBehindSouth_WhenCanMoveSouth_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.South), Is.True);
        }

        [Test]
        public void GivenCrateWithWallBehindSouth_WhenCanMoveSouth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            board.Tiles[4, 6] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.South), Is.False);
        }

        [Test]
        public void GivenCrateAtSouthEdge_WhenCanMoveSouth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 12);
            board.Tiles[4, 13] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.South), Is.False);
        }

        [Test]
        public void GivenCrateWithFloorBehindWest_WhenCanMoveWest_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.West), Is.True);
        }

        [Test]
        public void GivenCrateWithWallBehindWest_WhenCanMoveWest_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateCrateTile();
            board.Tiles[2, 4] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.West), Is.False);
        }

        [Test]
        public void GivenCrateAtWestEdge_WhenCanMoveWest_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 1, playerY: 4);
            board.Tiles[0, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.West), Is.False);
        }

        [Test]
        public void GivenCrateWithFloorBehindEast_WhenCanMoveEast_ThenReturnsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.East), Is.True);
        }

        [Test]
        public void GivenCrateWithWallBehindEast_WhenCanMoveEast_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateCrateTile();
            board.Tiles[6, 4] = BoardTestHelper.CreateWallTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.East), Is.False);
        }

        [Test]
        public void GivenCrateAtEastEdge_WhenCanMoveEast_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 14, playerY: 4);
            board.Tiles[15, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            Assert.That(_gameManager.CanMove(MovementDirection.East), Is.False);
        }

        [Test]
        public void GivenTwoAdjacentCratesNorth_WhenCanMoveNorth_ThenReturnsFalse()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            board.Tiles[4, 2] = BoardTestHelper.CreateCrateTile();
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

        [Test]
        public void GivenCrateAhead_WhenMovePlayerSouth_ThenPlayerMovesSouth()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.South);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(5));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerSouth_ThenCrateMovesForward()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.South);

            Assert.That(_gameManager.GetTile(4, 6).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerSouth_ThenCrateOriginalPositionBecomesFloor()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.South);

            Assert.That(_gameManager.GetTile(4, 5).Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerWest_ThenPlayerMovesWest()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.West);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(3));
            Assert.That(player.Location.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerWest_ThenCrateMovesForward()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.West);

            Assert.That(_gameManager.GetTile(2, 4).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerWest_ThenCrateOriginalPositionBecomesFloor()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.West);

            Assert.That(_gameManager.GetTile(3, 4).Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerEast_ThenPlayerMovesEast()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.East);

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(5));
            Assert.That(player.Location.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerEast_ThenCrateMovesForward()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.East);

            Assert.That(_gameManager.GetTile(6, 4).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCrateAhead_WhenMovePlayerEast_ThenCrateOriginalPositionBecomesFloor()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.East);

            Assert.That(_gameManager.GetTile(5, 4).Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenCratePushed_WhenMovePlayer_ThenMovesCountIsIncremented()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 3] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);

            _gameManager.MovePlayer(MovementDirection.North);

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(1));
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

        [Test]
        public void GivenTwoMoves_WhenUndoTwice_ThenPlayerReturnsToStartLocation()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);
            _gameManager.MovePlayer(MovementDirection.West);

            _gameManager.Undo();
            _gameManager.Undo();

            Player player = _gameManager.GetPlayer();
            Assert.That(player.Location.X, Is.EqualTo(4));
            Assert.That(player.Location.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenTwoMoves_WhenUndoTwice_ThenMovesCountIsZero()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.North);
            _gameManager.MovePlayer(MovementDirection.West);

            _gameManager.Undo();
            _gameManager.Undo();

            Assert.That(_gameManager.GetPlayer().MovesCount, Is.EqualTo(0));
        }

        [Test]
        public void GivenCratePushedSouth_WhenUndo_ThenCrateReturnsToOriginalPosition()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.South);

            _gameManager.Undo();

            Assert.That(_gameManager.GetTile(4, 5).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCratePushedSouth_WhenUndo_ThenCrateForwardPositionBecomesFloor()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.South);

            _gameManager.Undo();

            Assert.That(_gameManager.GetTile(4, 6).Id, Is.EqualTo(TileId.Floor));
        }

        [Test]
        public void GivenCratePushedWest_WhenUndo_ThenCrateReturnsToOriginalPosition()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[3, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.West);

            _gameManager.Undo();

            Assert.That(_gameManager.GetTile(3, 4).Id, Is.EqualTo(TileId.CrateOnFloor));
        }

        [Test]
        public void GivenCratePushedEast_WhenUndo_ThenCrateReturnsToOriginalPosition()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[5, 4] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.East);

            _gameManager.Undo();

            Assert.That(_gameManager.GetTile(5, 4).Id, Is.EqualTo(TileId.CrateOnFloor));
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

        [Test]
        public void GivenPlayerMovedSouth_WhenPeekUndo_ThenPlayerTargetIsOriginalLocation()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.South);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.PlayerTarget.X, Is.EqualTo(4));
            Assert.That(undoInfo.PlayerTarget.Y, Is.EqualTo(4));
        }

        [Test]
        public void GivenCratePushedSouth_WhenPeekUndo_ThenCratePushedIsTrue()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.South);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CratePushed, Is.True);
        }

        [Test]
        public void GivenCratePushedSouth_WhenPeekUndo_ThenCrateAnimStartIsWhereCrateEnded()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.South);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CrateAnimStart.X, Is.EqualTo(4));
            Assert.That(undoInfo.CrateAnimStart.Y, Is.EqualTo(6));
        }

        [Test]
        public void GivenCratePushedSouth_WhenPeekUndo_ThenCrateAnimEndIsWhereCrateCameFrom()
        {
            Board board = BoardTestHelper.CreateBoard(playerX: 4, playerY: 4);
            board.Tiles[4, 5] = BoardTestHelper.CreateCrateTile();
            StartGameWithBoard(board);
            _gameManager.MovePlayer(MovementDirection.South);

            UndoInfo undoInfo = _gameManager.PeekUndo();

            Assert.That(undoInfo.CrateAnimEnd.X, Is.EqualTo(4));
            Assert.That(undoInfo.CrateAnimEnd.Y, Is.EqualTo(5));
        }

        // -------------------------------------------------------------------------
        // GetTargets
        // -------------------------------------------------------------------------

        [Test]
        public void GivenBoardWithTargets_WhenGetTargets_ThenTargetsAreReturned()
        {
            Board board = BoardTestHelper.CreateBoard();
            board.Targets.Add(new Point2D(3, 5));
            board.Targets.Add(new Point2D(7, 2));
            StartGameWithBoard(board);

            List<Point2D> targets = _gameManager.GetTargets();

            Assert.That(targets.Count, Is.EqualTo(2));
        }

        [Test]
        public void GivenBoardWithNoTargets_WhenGetTargets_ThenEmptyListIsReturned()
        {
            Board board = BoardTestHelper.CreateBoard();
            StartGameWithBoard(board);

            List<Point2D> targets = _gameManager.GetTargets();

            Assert.That(targets, Is.Empty);
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
