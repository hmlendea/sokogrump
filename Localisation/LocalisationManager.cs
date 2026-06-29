using System.IO;
using System.Threading;

using NuciDAL.IO;

using SokoGrump.DataAccess.DataObjects;
using SokoGrump.Settings;

namespace SokoGrump.Localisation
{
    public class LocalisationManager
    {
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
            string localisationFile = ApplicationPaths.LocalisationFile("en");

            if (!File.Exists(localisationFile))
                return;

            JsonFileObject<LocalisationData> jsonManager = new();
            data = jsonManager.Read(localisationFile);
        }
    }
}
