namespace SokoGrump.Settings
{
    public class AudioSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the sound is enabled.
        /// </summary>
        /// <value>The sound toggle.</value>
        public bool SoundEnabled { get; set; }

        public AudioSettings()
        {
            SoundEnabled = true;
        }
    }
}
