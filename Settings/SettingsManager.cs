﻿using System.IO;
using System.Threading;
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
        static readonly Lock syncRoot = new();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SettingsManager Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (syncRoot)
                    {
                        instance ??= new SettingsManager();
                    }
                }

                return instance;
            }
        }

        public AudioSettings AudioSettings { get; set; }

        public GraphicsSettings GraphicsSettings { get; set; }

        public UserData UserData { get; set; }

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
            UserData = new UserData();
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadContent()
        {
            if (!File.Exists(ApplicationPaths.SettingsFile))
            {
                //tring logMessage = "Settings file is missing. Using default settings.";
                //Dictionary<LogInfoKey, string> logDetails = new Dictionary<LogInfoKey, string>
                //{
                //    { LogInfoKey.FileName, ApplicationPaths.SettingsFile }
                //};

                // TODO: Logging
                //LogManager.Instance.Warn(Operation.SettingsLoading, OperationStatus.Failure, logMessage, logDetails);

                SaveContent();
                return;
            }

            XmlFileObject<SettingsManager> xmlManager = new();
            SettingsManager storedSettings = xmlManager.Read(ApplicationPaths.SettingsFile);

            instance = storedSettings;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveContent()
        {
            XmlFileObject<SettingsManager> xmlManager = new();
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
