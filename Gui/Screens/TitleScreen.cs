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
        GuiMenuLink settingsLink;
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
                TargetScreen = typeof(GameplayScreen),
                Parameters = new object[] { default(int) }
            };
            settingsLink = new GuiMenuLink
            {
                Id = nameof(settingsLink),
                Text = "Settings",
                TargetScreen = typeof(SettingsScreen)
            };
            extiAction = new GuiMenuItem
            {
                Id = nameof(extiAction),
                Text = "Exit"
            };

            Items.Add(newGameLink);
            Items.Add(settingsLink);
            Items.Add(extiAction);

            base.DoLoadContent();
        }
    }
}
