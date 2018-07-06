namespace SokoGrump.GameLogic
{
    public enum TileType
    {
        /// <summary>
        /// The transparent.
        /// </summary>
        Transparent,
        /// <summary>
        /// The solid.
        /// </summary>
        Solid,
        /// <summary>
        /// The moveable.
        /// </summary>
        Moveable
    }

    public class Tile
    {
        readonly int id;
        TileType type;
        int variation;

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID { get { return id; } }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public TileType Type { get { return type; } }

        /// <summary>
        /// Gets or sets the variation.
        /// </summary>
        /// <value>The variation.</value>
        public int Variation
        {
            get { return variation; }
            set { variation = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="type">Type.</param>
        public Tile(int id, TileType type)
        {
            this.id = id;
            this.type = type;
            variation = 0;
        }
    }

    public static class Tiles
    {
        /// <summary>
        /// Gets by ID.
        /// </summary>
        /// <returns>The tile.</returns>
        /// <param name="id">Identifier.</param>
        public static Tile ByID(int id)
        {
            switch (id)
            {
                default:
                    //case 7: // Nothingness
                    return new Tile(id, TileType.Solid);

                case 0: // Terrain
                    return new Tile(id, TileType.Transparent);

                case 1: // Wall
                    return new Tile(id, TileType.Solid);

                case 2: // Box
                    return new Tile(id, TileType.Moveable);

                case 3: // Target
                    return new Tile(id, TileType.Transparent);

                case 5: // Completed - Box+Target
                    return new Tile(id, TileType.Moveable);
            }
        }
    }
}
