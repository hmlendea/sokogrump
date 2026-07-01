using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NuciDAL.IO;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Settings;

namespace SokoGrump.Localisation
{
    public class LocalisationManager : Singleton<LocalisationManager>
    {
        const string FallbackLanguage = "en";

        LocalisationData data;

        public string CurrentLanguage { get; private set; } = FallbackLanguage;

        public string TimeLabel => data.TimeLabel;
        public string MovesLabel => data.MovesLabel;
        public string LevelLabel => data.LevelLabel;

        public string NewGame => data.NewGame;
        public string Settings => data.Settings;
        public string ContinueGame => data.ContinueGame;

        public string Fullscreen => data.Fullscreen;
        public string Back => data.Back;
        public string LanguageSetting => data.LanguageSetting;

        public string RetryTooltip => data.RetryTooltip;
        public string UndoTooltip => data.UndoTooltip;

        public LocalisationManager()
        {
            data = new LocalisationData();
        }

        public void LoadContent()
        {
            string savedLanguage = SettingsManager.Instance.UserData.Language;
            JsonFileObject<LocalisationData> jsonManager = new();

            IEnumerable<string> candidates;

            if (string.IsNullOrEmpty(savedLanguage))
            {
                candidates = GetSystemLanguageCandidates();
            }
            else
            {
                candidates = [savedLanguage, FallbackLanguage];
            }

            foreach (string candidate in candidates)
            {
                string localisationFile = ApplicationPaths.LocalisationFile(candidate);

                if (File.Exists(localisationFile))
                {
                    data = jsonManager.Read(localisationFile);
                    CurrentLanguage = candidate;

                    if (string.IsNullOrEmpty(savedLanguage))
                    {
                        SettingsManager.Instance.UserData.Language = candidate;
                        SettingsManager.Instance.SaveContent();
                    }

                    return;
                }
            }

            CurrentLanguage = FallbackLanguage;

            if (string.IsNullOrEmpty(savedLanguage))
            {
                SettingsManager.Instance.UserData.Language = FallbackLanguage;
                SettingsManager.Instance.SaveContent();
            }
        }

        static IEnumerable<string> GetSystemLanguageCandidates()
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;

            return
            [
                culture.Name,
                culture.TwoLetterISOLanguageName,
                FallbackLanguage
            ];
        }
    }
}
