namespace SokoGrump.DataAccess.DataObjects
{
    public sealed class LocalisationData
    {
        public string TimeLabel { get; set; } = "Time";
        public string MovesLabel { get; set; } = "Moves";
        public string LevelLabel { get; set; } = "Level";

        public string NewGame { get; set; } = "New Game";
        public string Settings { get; set; } = "Settings";
        public string ContinueGame { get; set; } = "Continue Game";

        public string Fullscreen { get; set; } = "Fullscreen";
        public string Back { get; set; } = "Back";

        public string RetryTooltip { get; set; } = "Retry this level ('R' key)";
    }
}
