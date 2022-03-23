using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Gui.Controls;
using SokoGrump.Settings;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Editor screen.
    /// </summary>
    public class EditorScreen : Screen
    {
        IEditorManager editor;

        GuiEditorBoard editorBoard;

        public EditorScreen()
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            editor = new EditorManager();
            editor.LoadContent();

            editorBoard = new GuiEditorBoard(editor)
            {
                Size = new Size2D(
                    GameDefines.BoardWidth * GameDefines.MapTileSize,
                    GameDefines.BoardHeight * GameDefines.MapTileSize)
            };

            GuiManager.Instance.RegisterControls(editorBoard);
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            editor.UnloadContent();

            SettingsManager.Instance.SaveContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            editor.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Sets the properties of the child controls.
        /// </summary>
        void SetChildrenProperties()
        {
            editorBoard.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - editorBoard.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - editorBoard.Size.Height) / 2);
        }
    }
}
