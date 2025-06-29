﻿using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;

namespace SokoGrump.Gui.SpriteEffects
{
    public class CrateSpriteSheetEffect : SpriteSheetEffect
    {
        readonly IGameManager game;

        public Point2D TileLocation { get; set; }

        public CrateSpriteSheetEffect(IGameManager game)
        {
            FrameAmount = new Size2D(11, 1);

            this.game = game;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
            => CurrentFrame = new Point2D(game.GetTile(TileLocation.X, TileLocation.Y).Variation, 0);
    }
}
