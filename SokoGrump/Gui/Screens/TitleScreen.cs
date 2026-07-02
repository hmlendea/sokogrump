using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

using SokoGrump.Localisation;
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
            newGameLink = new GuiMenuLink
            {
                Id = nameof(newGameLink),
                Text = LocalisationManager.Instance.NewGame,
                TargetScreen = typeof(GameplayScreen),
                Parameters = [default(int)]
            };
            settingsLink = new GuiMenuLink
            {
                Id = nameof(settingsLink),
                Text = LocalisationManager.Instance.Settings,
                TargetScreen = typeof(SettingsScreen)
            };

            if (SettingsManager.Instance.UserData.LastLevel > 0)
            {
                continueGameLink = new GuiMenuLink
                {
                    Id = nameof(continueGameLink),
                    Text = LocalisationManager.Instance.ContinueGame,
                    TargetScreen = typeof(GameplayScreen),
                    Parameters = [SettingsManager.Instance.UserData.LastLevel]
                };

                Items.Add(continueGameLink);
            }

            Items.Add(newGameLink);
            Items.Add(settingsLink);

            base.DoLoadContent();
        }
    }
}
