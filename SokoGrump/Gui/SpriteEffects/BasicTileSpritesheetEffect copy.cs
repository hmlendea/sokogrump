using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;

namespace SokoGrump.Gui.SpriteEffects
{
    public class BasicTileSpriteSheetEffect : SpriteSheetEffect
    {
        public BasicTileSpriteSheetEffect()
            : base()
        {
            FrameAmount = new Size2D(3, 6);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            CurrentFrame = new Point2D(0, 0);
        }
    }
}
