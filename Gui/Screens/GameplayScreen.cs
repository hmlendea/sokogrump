using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Gui.Controls;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        IGameManager game;

        GuiButton retryButton;
        GuiInfoBar infoBar;
        GuiGameBoard gameBoard;

        readonly int level;

        public GameplayScreen(int level)
        {
            this.level = level;

            BackgroundColour = Colour.Black;
            ForegroundColour = Colour.White;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            game = new GameManager();
            game.LoadContent();
            game.NewGame(level);

            retryButton = new GuiButton
            {
                ContentFile = "Buttons/refresh",
                TooltipText = "Retry this level ('R' key)"
            };
            infoBar = new GuiInfoBar(game);
            gameBoard = new GuiGameBoard(game)
            {
                Size = new Size2D(
                    GameDefines.BoardWidth * GameDefines.MapTileSize,
                    GameDefines.BoardHeight * GameDefines.MapTileSize)
            };

            GuiManager.Instance.RegisterControls(retryButton, infoBar, gameBoard);
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            game.UnloadContent();
            UnregisterEvents();

            SettingsManager.Instance.SaveContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            game.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (game.Completed)
            {
                int nextLevel = game.Level + 1;

                if (File.Exists(Path.Combine("Levels", $"{nextLevel}.lvl")))
                {
                    ScreenManager.Instance.ChangeScreens(typeof(VictoryScreen), nextLevel);
                    SettingsManager.Instance.UserData.LastLevel = nextLevel;
                }
                else
                {
                    ScreenManager.Instance.ChangeScreens<GameFinishedScreen>();
                    SettingsManager.Instance.UserData.LastLevel = 0;
                }
            }

            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            retryButton.Clicked += OnRetryButtonPressed;
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            retryButton.Clicked -= OnRetryButtonPressed;
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
        }

        /// <summary>
        /// Sets the properties of the child controls.
        /// </summary>
        void SetChildrenProperties()
        {
            retryButton.Location = new Point2D(ScreenManager.Instance.Size.Width - GameDefines.MapTileSize, 0);
            retryButton.Size = new Size2D(GameDefines.MapTileSize, GameDefines.MapTileSize);
            retryButton.BackgroundColour = BackgroundColour;
            retryButton.ForegroundColour = ForegroundColour;

            infoBar.Location = Point2D.Empty;
            infoBar.Size = new Size2D(ScreenManager.Instance.Size.Width, 24);
            infoBar.BackgroundColour = Colour.Transparent;
            infoBar.ForegroundColour = ForegroundColour;

            gameBoard.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - gameBoard.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - gameBoard.Size.Height) / 2);
        }

        void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                case Keys.Up:
                    game.MovePlayer(MovementDirection.North);
                    break;

                case Keys.A:
                case Keys.Left:
                    game.MovePlayer(MovementDirection.West);
                    break;

                case Keys.S:
                case Keys.Down:
                    game.MovePlayer(MovementDirection.South);
                    break;

                case Keys.D:
                case Keys.Right:
                    game.MovePlayer(MovementDirection.East);
                    break;

                case Keys.R:
                    game.Retry();
                    break;
            }
        }

        void OnRetryButtonPressed(object sender, MouseButtonEventArgs e) => game.Retry();
    }
}
