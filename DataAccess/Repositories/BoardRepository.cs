using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using NuciDAL.Repositories;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Settings;

namespace SokoGrump.DataAccess.Repositories
{
    /// <summary>
    /// Board repository implementation.
    /// </summary>
    public class BoardRepository : IRepository<string, BoardEntity>
    {
        readonly string boardsDirectory;

        public int EntitiesCount => GetAll().Count();

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardRepository"/> class.
        /// </summary>
        /// <param name="boardsDirectory">File name.</param>
        public BoardRepository(string boardsDirectory)
        {
            this.boardsDirectory = boardsDirectory;
        }

        /// <summary>
        /// Adds the specified board.
        /// </summary>
        /// <param name="boardEntity">Board.</param>
        public void Add(BoardEntity boardEntity)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }

        public void TryAdd(BoardEntity boardEntity) { }

        public bool ContainsId(string id) => TryGet(id) is not null;

        /// <summary>
        /// Get the board with the specified identifier.
        /// </summary>
        /// <returns>The board.</returns>
        /// <param name="id">Identifier.</param>
        public BoardEntity Get(string id)
        {
            BoardEntity boardEntity = new BoardEntity();
            string boardFile = Path.Combine(boardsDirectory, id, "board.xml");
            string levelFile = Path.Combine("Levels", $"{id}.lvl");
            string[] rows = File.ReadAllLines(levelFile);

            boardEntity.Id = id;
            boardEntity.Tiles = new TileEntity[GameDefines.BoardWidth, GameDefines.BoardHeight];

            Dictionary<int, TileEntity> tileEntities = GetTileEntities();

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    int tileId = (int)char.GetNumericValue(rows[y][x]);

                    if (tileId == 4)
                    {
                        boardEntity.PlayerStartLocationX = x;
                        boardEntity.PlayerStartLocationY = y;
                        boardEntity.Tiles[x, y] = tileEntities[0];
                    }
                    else if (tileId == 6)
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

            // TODO: Logging
            //LogManager.Instance.Info(
            //    Operation.WorldLoading,
            //    OperationStatus.Success,
            //    new Dictionary<LogInfoKey, string>
            //    {
            //        { LogInfoKey.FileName, levelFile }
            //    });

            return boardEntity;
        }

        public BoardEntity TryGet(string id)
        {
            try
            {
                return Get(id);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all the boards.
        /// </summary>
        /// <returns>The boards</returns>
        public IEnumerable<BoardEntity> GetAll()
        {
            List<BoardEntity> boardEntities = new List<BoardEntity>();

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
        public void Update(BoardEntity boardEntity)
        {
            string boardFile = Path.Combine(boardsDirectory, boardEntity.Id, "board.xml");

            using (TextWriter writer = new StreamWriter(boardFile))
            {
                XmlSerializer xml = new XmlSerializer(typeof(BoardEntity));
                xml.Serialize(writer, boardEntity);
            }

            // TODO: Save the ProvinceMap and TerrainMap as well
        }

        public void TryUpdate(BoardEntity boardEntity)
        {
            try
            {
                Update(boardEntity);
            }
            catch { }
        }

        /// <summary>
        /// Removes the board with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            Directory.Delete(Path.Combine(boardsDirectory, id));
        }

        public void TryRemove(string id)
        {
            try
            {
                Remove(id);
            }
            catch { }
        }

        /// <summary>
        /// Removes the specified board.
        /// </summary>
        /// <param name="boardEntity">Board.</param>
        public void Remove(BoardEntity boardEntity)
        {
            Remove(boardEntity.Id);
        }

        public void TryRemove(BoardEntity boardEntity)
        {
            try
            {
                Remove(boardEntity);
            }
            catch { }
        }

        public void ApplyChanges() { }

        Dictionary<int, TileEntity> GetTileEntities()
        {
            Dictionary<int, TileEntity> tiles = new Dictionary<int, TileEntity>();

            TileEntity terrainTile = new TileEntity
            {
                Id = 0,
                SpriteSheet = "SpriteSheets/brick",
                TileType = "Walkable"
            };
            TileEntity wallTile = new TileEntity
            {
                Id = 1,
                SpriteSheet = "SpriteSheets/wall",
                TileType = "Solid"
            };
            TileEntity boxTile = new TileEntity
            {
                Id = 2,
                SpriteSheet = "SpriteSheets/crate",
                TileType = "Moveable"
            };
            TileEntity targetTile = new TileEntity
            {
                Id = 3,
                SpriteSheet = "Tiles/tile3/0",
                TileType = "Walkable"
            };
            TileEntity completedTargetTile = new TileEntity
            {
                Id = 5,
                SpriteSheet = "Tiles/tile5/0",
                TileType = "Moveable"
            };
            TileEntity voidTile = new TileEntity
            {
                Id = 7,
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
