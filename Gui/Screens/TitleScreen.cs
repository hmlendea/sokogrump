using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

using SokoGrump.Settings;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : MenuScreen
    {
        GuiMenuLink continueGameLink;
        GuiMenuLink newGameLink;
        GuiMenuLink settingsLink;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            continueGameLink = new GuiMenuLink
            {
                Id = nameof(continueGameLink),
                Text = "Continue Game",
                TargetScreen = typeof(GameplayScreen),
                Parameters = new object[] { SettingsManager.Instance.UserData.LastLevel }
            };
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

            Items.Add(continueGameLink);
            Items.Add(newGameLink);
            Items.Add(settingsLink);

            base.DoLoadContent();
        }
    }
}
