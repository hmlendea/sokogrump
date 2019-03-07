using System.Collections.Generic;

using NuciXNA.Primitives;

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
            Targets = new List<Point2D>();
        }
    }
}
