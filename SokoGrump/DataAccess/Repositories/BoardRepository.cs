using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using NuciDAL.Repositories;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.DataAccess.Repositories
{
    /// <summary>
    /// Board repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BoardRepository"/> class.
    /// </remarks>
    /// <param name="boardsDirectory">File name.</param>
    public sealed class BoardRepository(string boardsDirectory) : Repository<string, BoardEntity>
    {
        /// <summary>
        /// Get the board with the specified identifier.
        /// </summary>
        /// <returns>The board.</returns>
        /// <param name="id">Identifier.</param>
        public override BoardEntity Get(string id)
        {
            BoardEntity boardEntity = new();
            string levelFile = Path.Combine(boardsDirectory, $"{id}.lvl");
            string[] rows = File.ReadAllLines(levelFile);

            boardEntity.Id = id;
            boardEntity.Tiles = new TileEntity[GameDefines.BoardWidth, GameDefines.BoardHeight];

            Dictionary<int, TileEntity> tileEntities = GetTileEntities();

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    int tileId = (int)char.GetNumericValue(rows[y][x]);

                    if ((TileId)tileId == TileId.PlayerOnFloor)
                    {
                        boardEntity.PlayerStartLocationX = x;
                        boardEntity.PlayerStartLocationY = y;
                        boardEntity.Tiles[x, y] = tileEntities[0];
                    }
                    else if ((TileId)tileId == TileId.PlayerOnTarget)
                    {
                        boardEntity.PlayerStartLocationX = x;
                        boardEntity.PlayerStartLocationY = y;
                        boardEntity.Tiles[x, y] = tileEntities[3];
                    }
                    else
                    {
                        boardEntity.Tiles[x, y] = tileEntities[tileId];
                    }
                }
            }

            // TODO: Logging.
            return boardEntity;
        }

        /// <summary>
        /// Gets all the boards.
        /// </summary>
        /// <returns>The boards</returns>
        public override IEnumerable<BoardEntity> GetAll()
        {
            List<BoardEntity> boardEntities = [];

            foreach (string boardFile in Directory.GetFiles(boardsDirectory))
            {
                string boardId = Path.GetFileNameWithoutExtension(boardFile);

                BoardEntity boardEntity = Get(boardId);

                boardEntities.Add(boardEntity);
            }

            return boardEntities;
        }

        /// <summary>
        /// Updates the specified board.
        /// </summary>
        /// <param name="boardEntity">Board.</param>
        public override void Update(BoardEntity boardEntity)
        {
            string boardFile = Path.Combine(boardsDirectory, boardEntity.Id, "board.xml");

            using TextWriter writer = new StreamWriter(boardFile);
            XmlSerializer xml = new(typeof(BoardEntity));
            xml.Serialize(writer, boardEntity);
        }

        /// <summary>
        /// Removes the board with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public override void Remove(string id)
            => Directory.Delete(Path.Combine(boardsDirectory, id));

        public void ApplyChanges() { }

        private static Dictionary<int, TileEntity> GetTileEntities()
        {
            Dictionary<int, TileEntity> tiles = [];

            TileEntity terrainTile = new()
            {
                Id = (int)TileId.Floor,
                SpriteSheet = "SpriteSheets/brick",
                TileType = "Walkable"
            };
            TileEntity wallTile = new()
            {
                Id = (int)TileId.Wall,
                SpriteSheet = "SpriteSheets/wall",
                TileType = "Solid"
            };
            TileEntity boxTile = new()
            {
                Id = (int)TileId.CrateOnFloor,
                SpriteSheet = "SpriteSheets/crate",
                TileType = "Moveable"
            };
            TileEntity targetTile = new()
            {
                Id = (int)TileId.EmptyTarget,
                SpriteSheet = "Tiles/tile3/0",
                TileType = "Walkable"
            };
            TileEntity completedTargetTile = new()
            {
                Id = (int)TileId.CrateOnTarget,
                SpriteSheet = "Tiles/tile5/0",
                TileType = "Moveable"
            };
            TileEntity voidTile = new()
            {
                Id = (int)TileId.Void,
                SpriteSheet = "Tiles/tile7/0",
                TileType = "Solid"
            };

            tiles.Add(terrainTile.Id, terrainTile);
            tiles.Add(wallTile.Id, wallTile);
            tiles.Add(boxTile.Id, boxTile);
            tiles.Add(targetTile.Id, targetTile);
            tiles.Add(completedTargetTile.Id, completedTargetTile);
            tiles.Add(voidTile.Id, voidTile);

            return tiles;
        }
    }
}
