
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using NuciDAL.IO;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Localisation;
using SokoGrump.Settings;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Settings screen.
    /// </summary>
    public class SettingsScreen : MenuScreen
    {
        GuiMenuToggle fullScreenToggle;
        GuiMenuListSelector languageSelector;
        GuiMenuLink backLink;

        SortedDictionary<string, string> availableLanguages;
        bool initialLanguageSelectionPending;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            fullScreenToggle = new GuiMenuToggle
            {
                Id = nameof(fullScreenToggle),
                Text = LocalisationManager.Instance.Fullscreen
            };
            languageSelector = new GuiMenuListSelector
            {
                Id = nameof(languageSelector),
                Text = LocalisationManager.Instance.LanguageSetting
            };
            backLink = new GuiMenuLink
            {
                Id = nameof(backLink),
                Text = LocalisationManager.Instance.Back,
                TargetScreen = typeof(TitleScreen)
            };

            SortedDictionary<string, string> languages = GetAvailableLanguages();
            languageSelector.SetItems(new Dictionary<string, string>(languages));

            availableLanguages = languages;
            initialLanguageSelectionPending = true;

            Items.Add(fullScreenToggle);
            Items.Add(languageSelector);
            Items.Add(backLink);

            fullScreenToggle.SetState(SettingsManager.Instance.GraphicsSettings.Fullscreen);

            base.DoLoadContent();

            RegisterEvents();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            if (initialLanguageSelectionPending)
            {
                string currentLanguage = SettingsManager.Instance.UserData.Language;
                int languageIndex = availableLanguages.Keys
                    .ToList()
                    .IndexOf(currentLanguage);

                UnregisterEvents();
                languageSelector.SelectItemByIndex(Math.Max(0, languageIndex));
                RegisterEvents();

                initialLanguageSelectionPending = false;
            }

            base.DoUpdate(gameTime);
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
            languageSelector.SelectedItemChanged += OnLanguageSelectorSelectedItemChanged;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            fullScreenToggle.StateChanged -= OnFullscreenToggleStateChanged;
            languageSelector.SelectedItemChanged -= OnLanguageSelectorSelectedItemChanged;
        }

        void OnFullscreenToggleStateChanged(object sender, EventArgs e)
            => SettingsManager.Instance.GraphicsSettings.Fullscreen = fullScreenToggle.IsOn;

        void OnLanguageSelectorSelectedItemChanged(object sender, EventArgs e)
        {
            string selectedLanguage = languageSelector.SelectedKey;

            if (selectedLanguage == SettingsManager.Instance.UserData.Language)
            {
                return;
            }

            SettingsManager.Instance.UserData.Language = selectedLanguage;
            LocalisationManager.Instance.LoadContent();
            ScreenManager.Instance.ChangeScreens<SettingsScreen>();
        }

        static SortedDictionary<string, string> GetAvailableLanguages()
        {
            SortedDictionary<string, string> languages = new();
            JsonFileObject<LocalisationData> jsonManager = new();

            foreach (string file in Directory.GetFiles(ApplicationPaths.LocalisationDirectory, "*.json"))
            {
                string code = Path.GetFileNameWithoutExtension(file);
                LocalisationData locData = jsonManager.Read(file);
                languages[code] = locData.LanguageName;
            }

            return languages;
        }
    }
}
