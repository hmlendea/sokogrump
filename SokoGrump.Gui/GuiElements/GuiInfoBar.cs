using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;

namespace SokoGrump.Gui.GuiElements
{
    public sealed class GuiInfoBar : GuiElement
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

        public override void LoadContent()
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

            AddChild(background);
            AddChild(movesText);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            movesText.Text = $"Moves: {game.GetPlayer().MovesCount}";

            base.SetChildrenProperties();
        }
    }
}
