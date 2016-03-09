namespace SokoGrump.Game
{
    public enum TileType
    {
        Transparent,
        Solid,
        Moveable
    }

    public class Tile
    {
        readonly int id;
        TileType type;
        int variation;

        public int ID { get { return id; } }

        public TileType Type { get { return type; } }

        public int Variation
        {
            get { return variation; }
            set { variation = value; }
        }

        public Tile(int id, TileType type)
        {
            this.id = id;
            this.type = type;
            variation = 0;
        }
    }

    public static class Tiles
    {
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
