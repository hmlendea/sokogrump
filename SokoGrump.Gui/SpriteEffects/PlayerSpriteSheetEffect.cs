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

        public PlayerSpriteSheetEffect(IGameManager game)
            : base()
        {
            FrameAmount = new Size2D(3, 6);

            this.game = game;
        }

        public override void UpdateFrame(GameTime gameTime)
        {
            Player player = game.GetPlayer();

            if (player.Direction == MovementDirection.West)
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
