using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic;
using SokoGrump.Models;

namespace SokoGrump.Gui.SpriteEffects
{
    public class CrateSpriteSheetEffect : SpriteSheetEffect
    {
        readonly GameEngine game;

        public Point2D TileLocation { get; set; }

        public CrateSpriteSheetEffect(GameEngine game)
        {
            FrameAmount = new Size2D(11, 1);

            this.game = game;
        }

        public override void UpdateFrame(GameTime gameTime)
        {
            Tile tile = game.GetTile(TileLocation.X, TileLocation.Y);

            CurrentFrame = new Point2D(tile.Variation, 0);
        }
    }
}
