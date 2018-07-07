using NuciXNA.DataAccess.DataObjects;

namespace SokoGrump.DataAccess.DataObjects
{
    /// <summary>
    /// Board data entity.
    /// </summary>
    public sealed class BoardEntity : EntityBase
    {
        public int PlayerStartLocationX { get; set; }

        public int PlayerStartLocationY { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public TileEntity[,] Tiles { get; set; }
    }
}
