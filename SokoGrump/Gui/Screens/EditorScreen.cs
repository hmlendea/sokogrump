using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
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

        GuiImage saveButton;

        GuiTileButton wallTileButton;
        GuiTileButton terrainTileButton;
        GuiTileButton targetTileButton;
        GuiImage playerTileButton;
        GuiTileButton emptyCrateTileButton;
        GuiTileButton filledCrateTileButton;

        int selectedTileId = 1;

        public EditorScreen()
        {
            BackgroundColour = Colour.DimGray;
        }

        protected override void DoLoadContent()
        {
            editor = new EditorManager();
            editor.LoadContent();

            Size2D buttonSize = new Size2D(GameDefines.MapTileSize, GameDefines.MapTileSize);

            editorBoard = new GuiEditorBoard(editor)
            {
                Size = new Size2D(
                    GameDefines.BoardWidth * GameDefines.MapTileSize,
                    GameDefines.BoardHeight * GameDefines.MapTileSize)
            };

            saveButton = new GuiImage
            {
                Location = new Point2D(ScreenManager.Instance.Size.Width - GameDefines.MapTileSize, 0),
                ContentFile = "Buttons/save",
                Size = buttonSize
            };

            wallTileButton = new GuiTileButton(1)
            {
                Location = new Point2D(0, 0),
                Size = buttonSize
            };

            terrainTileButton = new GuiTileButton(0)
            {
                Location = new Point2D(0, GameDefines.MapTileSize),
                Size = buttonSize
            };

            playerTileButton = new GuiImage()
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 2),
                ContentFile = "Tiles/player/player",
                Size = buttonSize
            };

            targetTileButton = new GuiTileButton(3)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 3),
                Size = buttonSize
            };

            emptyCrateTileButton = new GuiTileButton(2)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 4),
                Size = buttonSize
            };

            filledCrateTileButton = new GuiTileButton(5)
            {
                Location = new Point2D(0, GameDefines.MapTileSize * 5),
                Size = buttonSize
            };

            GuiManager.Instance.RegisterControls(
                editorBoard,
                saveButton,
                wallTileButton,
                terrainTileButton,
                playerTileButton,
                targetTileButton,
                emptyCrateTileButton,
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
            saveButton.Clicked += OnSaveButtonClicked;

            wallTileButton.Clicked += delegate { selectedTileId = wallTileButton.TileId; };
            terrainTileButton.Clicked += delegate { selectedTileId = terrainTileButton.TileId; };
            playerTileButton.Clicked += delegate { selectedTileId = 4; };
            targetTileButton.Clicked += delegate { selectedTileId = targetTileButton.TileId; };
            emptyCrateTileButton.Clicked += delegate { selectedTileId = emptyCrateTileButton.TileId; };
            filledCrateTileButton.Clicked += delegate { selectedTileId = filledCrateTileButton.TileId; };
        }

        void UnregisterEvents()
        {
            editorBoard.MouseButtonPressed -= OnEditorBoardMouseButtonPressed;
            saveButton.Clicked -= OnSaveButtonClicked;
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

        void OnSaveButtonClicked(object sender, MouseButtonEventArgs e)
        {
            editor.SaveContent();
        }
    }
}
