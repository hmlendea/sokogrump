using System.IO;
using System.Collections.Generic;

using NuciLog;
using NuciLog.Enumerations;
using NuciXNA.DataAccess.IO;
using NuciXNA.Graphics;

namespace SokoGrump.Settings
{
    /// <summary>
    /// Settings manager.
    /// </summary>
    public class SettingsManager
    {
        static volatile SettingsManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SettingsManager();
                        }
                    }
                }

                return instance;
            }
        }

        public AudioSettings AudioSettings { get; set; }

        public GraphicsSettings GraphicsSettings { get; set; }

        /// <summary>
        /// Gets or sets the debug mode.
        /// </summary>
        /// <value>The debug mode.</value>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public SettingsManager()
        {
            AudioSettings = new AudioSettings();
            GraphicsSettings = new GraphicsSettings();
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadContent()
        {
            if (!File.Exists(ApplicationPaths.SettingsFile))
            {
                string logMessage = "Settings file is missing. Using default settings.";
                Dictionary<LogInfoKey, string> logDetails = new Dictionary<LogInfoKey, string>
                {
                    { LogInfoKey.FileName, ApplicationPaths.SettingsFile }
                };

                LogManager.Instance.Warn(Operation.SettingsLoading, OperationStatus.Failure, logMessage, logDetails);

                SaveContent();
                return;
            }

            XmlFileObject<SettingsManager> xmlManager = new XmlFileObject<SettingsManager>();
            SettingsManager storedSettings = xmlManager.Read(ApplicationPaths.SettingsFile);

            instance = storedSettings;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveContent()
        {
            XmlFileObject<SettingsManager> xmlManager = new XmlFileObject<SettingsManager>();
            xmlManager.Write(ApplicationPaths.SettingsFile, this);
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public void Update()
        {
            bool graphicsChanged = false;

            if (GraphicsManager.Instance.Graphics.IsFullScreen != GraphicsSettings.Fullscreen)
            {
                GraphicsManager.Instance.Graphics.IsFullScreen = GraphicsSettings.Fullscreen;

                graphicsChanged = true;
            }

            if (GraphicsManager.Instance.Graphics.PreferredBackBufferWidth != GraphicsSettings.Resolution.Width ||
                GraphicsManager.Instance.Graphics.PreferredBackBufferHeight != GraphicsSettings.Resolution.Height)
            {
                GraphicsManager.Instance.Graphics.PreferredBackBufferWidth = GraphicsSettings.Resolution.Width;
                GraphicsManager.Instance.Graphics.PreferredBackBufferHeight = GraphicsSettings.Resolution.Height;

                graphicsChanged = true;
            }

            if (graphicsChanged)
            {
                GraphicsManager.Instance.Graphics.ApplyChanges();
            }
        }
    }
}
