using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
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

        GuiTileButton wallTileButton;
        GuiTileButton terrainTileButton;
        GuiTileButton targetTileButton;
        GuiTileButton emptyCrateTileButton;
        GuiTileButton filledCrateTileButton;

        int selectedTileId = 1;

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

            wallTileButton = new GuiTileButton(1)
            {
                Location = new Point2D(0, 0)
            };

            terrainTileButton = new GuiTileButton(0)
            {
                Location = new Point2D(0, GameDefines.MapTileSize)
            };

            targetTileButton = new GuiTileButton(3)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 2)
            };

            emptyCrateTileButton = new GuiTileButton(2)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 3)
            };

            filledCrateTileButton = new GuiTileButton(5)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 4)
            };

            GuiManager.Instance.RegisterControls(
                editorBoard,
                terrainTileButton,
                wallTileButton,
                emptyCrateTileButton,
                targetTileButton,
                filledCrateTileButton);

            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            editor.UnloadContent();

            SettingsManager.Instance.SaveContent();

            UnregisterEvents();
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

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            wallTileButton.Clicked += OnTileButtonClicked;
            terrainTileButton.Clicked += OnTileButtonClicked;
            targetTileButton.Clicked += OnTileButtonClicked;
            emptyCrateTileButton.Clicked += OnTileButtonClicked;
            filledCrateTileButton.Clicked += OnTileButtonClicked;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            wallTileButton.Clicked -= OnTileButtonClicked;
            terrainTileButton.Clicked -= OnTileButtonClicked;
            targetTileButton.Clicked -= OnTileButtonClicked;
            emptyCrateTileButton.Clicked -= OnTileButtonClicked;
            filledCrateTileButton.Clicked -= OnTileButtonClicked;
        }

        void OnTileButtonClicked(object sender, MouseButtonEventArgs eventArgs)
        {
            GuiTileButton tileButton = (GuiTileButton)sender;
            selectedTileId = tileButton.TileId;
        }
    }
}
