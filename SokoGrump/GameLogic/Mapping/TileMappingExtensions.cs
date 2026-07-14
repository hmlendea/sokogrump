using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Models;

namespace SokoGrump.GameLogic.Mapping
{
    /// <summary>
    /// World Tile mapping extensions for converting between entities and domain models.
    /// </summary>
    static class TileMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="tileEntity">World Tile entity.</param>
        internal static Tile ToDomainModel(this TileEntity tileEntity) => new()
        {
            Id = (TileId)tileEntity.Id,
            SpriteSheet = tileEntity.SpriteSheet,
            TileType = Enum.Parse<TileType>(tileEntity.TileType)
        };

        /// <summary>
        /// Converts the domain model into a data object.
        /// </summary>
        /// <returns>The data object.</returns>
        /// <param name="tile">World Tile.</param>
        internal static TileEntity ToDataObject(this Tile tile) => new()
        {
            Id = (int)tile.Id,
            SpriteSheet = tile.SpriteSheet,
            TileType = tile.TileType.ToString()
        };

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="tileEntities">World Tile entities.</param>
        internal static IEnumerable<Tile> ToDomainModels(this IEnumerable<TileEntity> tileEntities)
            => tileEntities.Select(tileEntity => tileEntity.ToDomainModel());

        /// <summary>
        /// Converts the domain models into data objects.
        /// </summary>
        /// <returns>The data objects.</returns>
        /// <param name="tiles">World Tiles.</param>
        internal static IEnumerable<TileEntity> ToDataObjects(this IEnumerable<Tile> tiles)
            => tiles.Select(tile => tile.ToDataObject());

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="tileEntities">World Tile entities.</param>
        internal static Tile[,] ToDomainModels(this TileEntity[,] tileEntities)
        {
            int width = tileEntities.GetLength(0);
            int height = tileEntities.GetLength(1);

            Tile[,] tiles = new Tile[width, height];

            Parallel.For(
                0,
                height,
                rowIndex => Parallel.For(
                    0,
                    width,
                    columnIndex => tiles[columnIndex, rowIndex] = tileEntities[columnIndex, rowIndex].ToDomainModel()));

            return tiles;
        }

        /// <summary>
        /// Converts the domain models into data objects.
        /// </summary>
        /// <returns>The data objects.</returns>
        /// <param name="tiles">World Tiles.</param>
        internal static TileEntity[,] ToDataObjects(this Tile[,] tiles)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);

            TileEntity[,] tileEntities = new TileEntity[width, height];

            Parallel.For(
                0,
                height,
                rowIndex => Parallel.For(
                    0,
                    width,
                    columnIndex => tileEntities[columnIndex, rowIndex] = tiles[columnIndex, rowIndex].ToDataObject()));

            return tileEntities;
        }
    }
}
