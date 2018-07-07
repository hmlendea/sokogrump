using System.Collections.Generic;
using System.Linq;

using NuciXNA.Primitives;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Models;

namespace SokoGrump.GameLogic.Mapping
{
    /// <summary>
    /// Board mapping extensions for converting between entities and domain models.
    /// </summary>
    static class BoardMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="boardEntity">Board entity.</param>
        internal static Board ToDomainModel(this BoardEntity boardEntity)
        {
            Board board = new Board
            {
                Id = boardEntity.Id,
                PlayerStartLocation = new Point2D(
                    boardEntity.PlayerStartLocationX,
                    boardEntity.PlayerStartLocationY),
                Tiles = boardEntity.Tiles.ToDomainModels()
            };

            return board;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="board">Board.</param>
        internal static BoardEntity ToEntity(this Board board)
        {
            BoardEntity boardEntity = new BoardEntity
            {
                Id = board.Id,
                PlayerStartLocationX = board.PlayerStartLocation.X,
                PlayerStartLocationY = board.PlayerStartLocation.Y,
                Tiles = board.Tiles.ToEntities()
            };

            return boardEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="boardEntities">Board entities.</param>
        internal static IEnumerable<Board> ToDomainModels(this IEnumerable<BoardEntity> boardEntities)
        {
            IEnumerable<Board> boards = boardEntities.Select(boardEntity => boardEntity.ToDomainModel());

            return boards;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="boards">Boards.</param>
        internal static IEnumerable<BoardEntity> ToEntities(this IEnumerable<Board> boards)
        {
            IEnumerable<BoardEntity> boardEntities = boards.Select(board => board.ToEntity());

            return boardEntities;
        }
    }
}
