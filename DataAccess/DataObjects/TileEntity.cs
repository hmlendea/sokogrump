using NuciXNA.DataAccess.DataObjects;

namespace SokoGrump.DataAccess.DataObjects
{
    /// <summary>
    /// World tile entity.
    /// </summary>
    public sealed class TileEntity : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the sprite sheet.
        /// </summary>
        /// <value>The sprite sheet.</value>
        public string SpriteSheet { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public string TileType { get; set; }
    }
}
