using System.IO;
using NuciDAL.IO;
using NuciXNA.Graphics;

namespace SokoGrump.Settings
{
    /// <summary>
    /// Settings manager.
    /// </summary>
    public sealed class SettingsManager : Singleton<SettingsManager>
    {
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
                SaveContent();
                return;
            }

            XmlFileObject<SettingsManager> xmlManager = new();
            SettingsManager storedSettings = xmlManager.Read(ApplicationPaths.SettingsFile);

            SetInstance(storedSettings);
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
