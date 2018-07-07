namespace SokoGrump.Models
{
    public class Tile
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sprite sheet.
        /// </summary>
        /// <value>The sprite sheet.</value>
        public string SpriteSheet { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public TileType TileType { get; set; }

        /// <summary>
        /// Gets or sets the variation.
        /// </summary>
        /// <value>The variation.</value>
        public int Variation { get; set; }
    }
}
