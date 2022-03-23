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

            Size2D tileButtonSize = new Size2D(GameDefines.MapTileSize, GameDefines.MapTileSize);

            wallTileButton = new GuiTileButton(1)
            {
                Location = new Point2D(0, 0),
                Size = tileButtonSize
            };

            terrainTileButton = new GuiTileButton(0)
            {
                Location = new Point2D(0, GameDefines.MapTileSize),
                Size = tileButtonSize
            };

            targetTileButton = new GuiTileButton(3)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 2),
                Size = tileButtonSize
            };

            emptyCrateTileButton = new GuiTileButton(2)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 3),
                Size = tileButtonSize
            };

            filledCrateTileButton = new GuiTileButton(5)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 4),
                Size = tileButtonSize
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

        protected override void DoUnloadContent()
        {
            editor.UnloadContent();

            SettingsManager.Instance.SaveContent();

            UnregisterEvents();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            editor.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            SetChildrenProperties();
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void SetChildrenProperties()
        {
            editorBoard.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - editorBoard.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - editorBoard.Size.Height) / 2);
        }

        void RegisterEvents()
        {
            editorBoard.MouseButtonPressed += OnEditorBoardMouseButtonPressed;

            wallTileButton.Clicked += delegate { selectedTileId = wallTileButton.TileId; };
            terrainTileButton.Clicked += delegate { selectedTileId = terrainTileButton.TileId; };
            targetTileButton.Clicked += delegate { selectedTileId = targetTileButton.TileId; };
            emptyCrateTileButton.Clicked += delegate { selectedTileId = emptyCrateTileButton.TileId; };
            filledCrateTileButton.Clicked += delegate { selectedTileId = filledCrateTileButton.TileId; };
        }

        void UnregisterEvents()
        {
            editorBoard.MouseButtonPressed -= OnEditorBoardMouseButtonPressed;
        }

        void OnEditorBoardMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Point2D tileLocation = new Point2D(
                (e.Location.X - editorBoard.Location.X) / GameDefines.MapTileSize,
                (e.Location.Y - editorBoard.Location.Y) / GameDefines.MapTileSize);

            if (e.Button == MouseButton.Left)
            {
                editor.SetTile(tileLocation.X, tileLocation.Y, selectedTileId);
            }
            else if (e.Button == MouseButton.Right)
            {
                editor.SetTile(tileLocation.X, tileLocation.Y, 7);
            }
        }
    }
}
