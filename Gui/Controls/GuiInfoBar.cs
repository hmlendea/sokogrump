using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;

namespace SokoGrump.Gui.Controls
{
    public sealed class GuiInfoBar : GuiControl
    {
        readonly IGameManager game;

        GuiText movesText;
        GuiText levelText;

        public GuiInfoBar(IGameManager game)
        {
            this.game = game;

            FontName = "InfoBarFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            movesText = new GuiText
            {
                Location = new Point2D(0,0),
                Size= new Size2D(200, Size.Height),
                HorizontalAlignment = Alignment.Beginning,
                VerticalAlignment = Alignment.Middle
            };
            levelText = new GuiText
            {
                Size = new Size2D(200, Size.Height),
                HorizontalAlignment = Alignment.Middle,
                VerticalAlignment = Alignment.Middle
            };

            RegisterChildren(movesText, levelText);
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent() { }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime) => SetChildrenProperties();

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch) { }

        void SetChildrenProperties()
        {
            movesText.BackgroundColour = BackgroundColour;
            movesText.ForegroundColour = ForegroundColour;

            levelText.BackgroundColour = BackgroundColour;
            levelText.ForegroundColour = ForegroundColour;

            movesText.Text = $"Moves: {game.GetPlayer().MovesCount}";
            levelText.Text = $"Level {game.Level}";

            levelText.Location = new Point2D(
                (Size.Width - levelText.Size.Width) / 2,
                (Size.Height - levelText.Size.Height) / 2);
        }
    }
}
