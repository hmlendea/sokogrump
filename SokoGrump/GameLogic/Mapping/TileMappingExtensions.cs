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
        internal static Tile ToDomainModel(this TileEntity tileEntity)
        {
            Tile tile = new Tile
            {
                Id = tileEntity.Id,
                SpriteSheet = tileEntity.SpriteSheet,
                TileType = (TileType)Enum.Parse(typeof(TileType), tileEntity.TileType)
            };

            return tile;
        }

        /// <summary>
        /// Converts the domain model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="tile">World Tile.</param>
        internal static TileEntity ToEntity(this Tile tile)
        {
            TileEntity worldEntity = new TileEntity
            {
                Id = tile.Id,
                SpriteSheet = tile.SpriteSheet,
                TileType = tile.TileType.ToString()
            };

            return worldEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="tileEntities">World Tile entities.</param>
        internal static IEnumerable<Tile> ToDomainModels(this IEnumerable<TileEntity> tileEntities)
        {
            IEnumerable<Tile> tiles = tileEntities.Select(tileEntity => tileEntity.ToDomainModel());

            return tiles;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="tiles">World Tiles.</param>
        internal static IEnumerable<TileEntity> ToEntities(this IEnumerable<Tile> tiles)
        {
            IEnumerable<TileEntity> tileEntities = tiles.Select(tile => tile.ToEntity());

            return tileEntities;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="tileEntities">World Tile entities.</param>
        internal static Tile[,] ToDomainModels(this TileEntity[,] tileEntities)
        {
            int w = tileEntities.GetLength(0);
            int h = tileEntities.GetLength(1);

            Tile[,] tiles = new Tile[w, h];

            Parallel.For(0, h, y => Parallel.For(0, w, x => tiles[x, y] = tileEntities[x, y].ToDomainModel()));

            return tiles;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="tiles">World Tiles.</param>
        internal static TileEntity[,] ToEntities(this Tile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);

            TileEntity[,] tileEntities = new TileEntity[w, h];

            Parallel.For(0, h, y => Parallel.For(0, w, x => tileEntities[x, y] = tiles[x, y].ToEntity()));

            return tileEntities;
        }
    }
}
