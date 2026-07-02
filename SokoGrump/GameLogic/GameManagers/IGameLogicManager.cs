namespace SokoGrump.GameLogic.GameManagers
{
    public interface IGameLogicManager
    {
        /// <summary>
        /// Loads the content;
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unloads the content;
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Updates the content;
        /// </summary>
        void Update(double elapsedMiliseconds);
    }
}
