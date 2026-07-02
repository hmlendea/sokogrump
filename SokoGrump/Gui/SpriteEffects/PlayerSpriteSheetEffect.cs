using System.Collections.Generic;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;

namespace SokoGrump.Gui.SpriteEffects
{
    public class PlayerSpriteSheetEffect : SpriteSheetEffect
    {
        const int SpriteSheetColumns = 3;
        const int SpriteSheetRows = 6;

        static readonly Dictionary<MovementDirection, Point2D> DirectionFrames = new()
        {
            { MovementDirection.South, new Point2D(0, 0) },
            { MovementDirection.North, new Point2D(1, 0) },
            { MovementDirection.East,  new Point2D(0, 1) },
            { MovementDirection.West,  new Point2D(1, 1) },
        };

        readonly IGameManager game;

        public PlayerSpriteSheetEffect(IGameManager game) : base()
        {
            FrameAmount = new Size2D(SpriteSheetColumns, SpriteSheetRows);

            this.game = game;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            Player player = game.GetPlayer();

            if (DirectionFrames.TryGetValue(player.Direction, out Point2D frame))
            {
                CurrentFrame = frame;
            }
        }
    }
}
