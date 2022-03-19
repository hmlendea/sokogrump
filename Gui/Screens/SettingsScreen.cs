
using System;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

using SokoGrump.Settings;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Settings screen.
    /// </summary>
    public class SettingsScreen : MenuScreen
    {
        GuiMenuToggle fullScreenToggle;
        GuiMenuLink backLink;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            fullScreenToggle = new GuiMenuToggle
            {
                Id = nameof(fullScreenToggle),
                Text = "Fullscreen"
            };
            backLink = new GuiMenuLink
            {
                Id = nameof(backLink),
                Text = "Back",
                TargetScreen = typeof(TitleScreen)
            };
            
            Items.Add(fullScreenToggle);
            Items.Add(backLink);

            RegisterEvents();
            
            fullScreenToggle.SetState(SettingsManager.Instance.GraphicsSettings.Fullscreen);

            base.DoLoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            SettingsManager.Instance.SaveContent();

            UnregisterEvents();

            base.DoUnloadContent();
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            fullScreenToggle.StateChanged += OnFullscreenToggleStateChanged;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            fullScreenToggle.StateChanged -= OnFullscreenToggleStateChanged;
        }

        void OnFullscreenToggleStateChanged(object sender, EventArgs e)
        {
            SettingsManager.Instance.GraphicsSettings.Fullscreen = fullScreenToggle.IsOn;
        }
    }
}
