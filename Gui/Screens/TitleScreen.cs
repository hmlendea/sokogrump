using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : MenuScreen
    {
        GuiMenuLink newGameLink;
        GuiMenuItem extiAction;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            newGameLink = new GuiMenuLink
            {
                Id = nameof(newGameLink),
                Text = "New Game",
                TargetScreen = typeof(GameplayScreen)
            };
            extiAction = new GuiMenuItem
            {
                Id = nameof(extiAction),
                Text = "Exit"
            };

            Items.Add(newGameLink);
            Items.Add(extiAction);

            base.DoLoadContent();
        }
    }
}
