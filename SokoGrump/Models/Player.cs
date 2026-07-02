using NuciXNA.Primitives;

namespace SokoGrump.Models
{
    public sealed class Player : ModelBase
    {
        public Point2D Location { get; set; }

        public MovementDirection Direction { get; set; }

        public int MovesCount { get; set; }
    }
}
