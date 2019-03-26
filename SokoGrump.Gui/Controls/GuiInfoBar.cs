using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;

namespace SokoGrump.Gui.Controls
{
    public sealed class GuiInfoBar : GuiControl
    {
        readonly IGameManager game;

        GuiImage background;
        GuiText movesText;

        public GuiInfoBar(IGameManager game)
        {
            this.game = game;

            BackgroundColour = Colour.Black;
            FontName = "InfoBarFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                TintColour = BackgroundColour,
                ContentFile = "ScreenManager/FillImage"
            };
            movesText = new GuiText
            {
                Location = new Point2D(0,0),
                Size= new Size2D(100, Size.Height),
                BackgroundColour = Colour.Transparent,
                ForegroundColour = Colour.White
            };

            RegisterChildren(background, movesText);
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {

        }
        
        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {

        }
        
        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void SetChildrenProperties()
        {
            movesText.Text = $"Moves: {game.GetPlayer().MovesCount}";
        }
    }
}
