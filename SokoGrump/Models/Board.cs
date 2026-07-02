using System.Collections.Generic;

using NuciXNA.Primitives;

using SokoGrump.Settings;

namespace SokoGrump.Models
{
    public class Board : ModelBase
    {
        /// <summary>
        /// Gets or sets the player start location.
        /// </summary>
        /// <value>The player start location.</value>
        public Point2D PlayerStartLocation { get; set; }

        public List<Point2D> Targets { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public Tile[,] Tiles { get; set; }

        public Board()
        {
            Targets = [];
        }

        public Board Clone()
        {
            Board clone = new()
            {
                PlayerStartLocation = PlayerStartLocation,
                Targets = [.. Targets],
                Tiles = new Tile[GameDefines.BoardWidth, GameDefines.BoardHeight]
            };

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    clone.Tiles[x, y] = Tiles[x, y].Clone();
                }
            }

            return clone;
        }
    }
}
