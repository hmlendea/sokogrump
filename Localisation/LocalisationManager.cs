using System.Globalization;
using System.IO;
using System.Threading;

using NuciDAL.IO;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Settings;

namespace SokoGrump.Localisation
{
    public class LocalisationManager
    {
        const string FallbackLanguage = "en";

        static volatile LocalisationManager instance;
        static readonly Lock syncRoot = new();

        LocalisationData data;

        public static LocalisationManager Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (syncRoot)
                    {
                        instance ??= new LocalisationManager();
                    }
                }

                return instance;
            }
        }

        public string TimeLabel => data.TimeLabel;
        public string MovesLabel => data.MovesLabel;
        public string LevelLabel => data.LevelLabel;

        public string NewGame => data.NewGame;
        public string Settings => data.Settings;
        public string ContinueGame => data.ContinueGame;

        public string Fullscreen => data.Fullscreen;
        public string Back => data.Back;

        public string RetryTooltip => data.RetryTooltip;

        public LocalisationManager()
        {
            data = new LocalisationData();
        }

        public void LoadContent()
        {
            JsonFileObject<LocalisationData> jsonManager = new();

            foreach (string candidate in GetLanguageCandidates())
            {
                string localisationFile = ApplicationPaths.LocalisationFile(candidate);

                if (File.Exists(localisationFile))
                {
                    data = jsonManager.Read(localisationFile);
                    return;
                }
            }
        }

        static string[] GetLanguageCandidates()
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;

            return
            [
                culture.Name,                // e.g. "ro-RO"
                culture.TwoLetterISOLanguageName, // e.g. "ro"
                FallbackLanguage
            ];
        }
    }
}
