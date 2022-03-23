using System;
using System.IO;
using System.Reflection;

namespace SokoGrump.Settings
{
    /// <summary>
    /// Application paths.
    /// </summary>
    public static class ApplicationPaths
    {
        static readonly string rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets the user data directory.
        /// </summary>
        /// <value>The user data directory.</value>
        public static string UserDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SokoGrump");
        
        /// <summary>
        /// Gets the logs directory path.
        /// </summary>
        /// <value>The path to the logs directory.</value>
        public static string LogsDirectory => Path.Combine(UserDataDirectory, "Logs");

        /// <summary>
        /// Gets the options file.
        /// </summary>
        /// <value>The options file.</value>
        public static string SettingsFile => Path.Combine(UserDataDirectory, "Settings.xml");

        /// <summary>
        /// Gets the save file path.
        /// </summary>
        /// <value>The path to the save file.</value>
        public static string SaveFile => Path.Combine(UserDataDirectory, "progress.sav");

        /// <summary>
        /// Gets the word lists directory.
        /// </summary>
        /// <value>The word lists directory.</value>
        public static string LevelsDirectory => Path.Combine(rootDirectory, "Levels");
    }
}
