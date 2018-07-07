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

        /// <summary>
        /// Gets or sets the number of remaining targets.
        /// </summary>
        /// <value>The remaining targets.</value>
        public int TargetsLeft { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public Tile[,] Tiles { get; set; }
    }
}
