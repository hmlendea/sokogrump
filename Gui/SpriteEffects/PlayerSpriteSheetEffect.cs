using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;

namespace SokoGrump.Gui.SpriteEffects
{
    public class PlayerSpriteSheetEffect : SpriteSheetEffect
    {
        readonly IGameManager game;

        public PlayerSpriteSheetEffect(IGameManager game) : base()
        {
            FrameAmount = new Size2D(3, 6);

            this.game = game;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            if (game.GetPlayer().Direction.Equals(MovementDirection.West))
            {
                CurrentFrame = new Point2D(0, 0);
            }
            else
            {
                CurrentFrame = new Point2D(1, 0);
            }
        }
    }
}
