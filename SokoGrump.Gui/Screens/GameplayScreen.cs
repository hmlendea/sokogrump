using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Gui.GuiElements;
using SokoGrump.Models;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        IGameManager game;

        GuiInfoBar infoBar;
        GuiGameBoard gameBoard;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            game = new GameManager();
            game.LoadContent();

            int level = 0;

            if (ScreenArgs != null && ScreenArgs.Length > 0)
            {
                level = int.Parse(ScreenArgs[0]);
            }

            game.NewGame(level);

            infoBar = new GuiInfoBar(game)
            {
                Location = Point2D.Empty,
                Size = new Size2D(ScreenManager.Instance.Size.Width, 24)
            };
            gameBoard = new GuiGameBoard(game)
            {
                Size = new Size2D(
                    ScreenManager.Instance.Size.Width,
                    ScreenManager.Instance.Size.Height - 24),
                Location = new Point2D(0, 24)
            };

            GuiManager.Instance.GuiElements.Add(gameBoard);
            GuiManager.Instance.GuiElements.Add(infoBar);

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            game.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            game.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (game.Completed)
            {

                if (File.Exists(Path.Combine("Levels", (game.Level + 1) + ".lvl")))
                {
                    ScreenManager.Instance.ChangeScreens(typeof(VictoryScreen), new string[] { $"{game.Level + 1}" });
                }
            }
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            InputManager.Instance.KeyboardKeyPressed += InputManager_KeyboardKeyPressed;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            InputManager.Instance.KeyboardKeyPressed -= InputManager_KeyboardKeyPressed;
        }

        protected override void SetChildrenProperties()
        {
            infoBar.Size = new Size2D(ScreenManager.Instance.Size.Width, 24);

            base.SetChildrenProperties();
        }

        void InputManager_KeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
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
    }
}
