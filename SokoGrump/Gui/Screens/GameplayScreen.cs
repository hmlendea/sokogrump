using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using SokoGrump.GameLogic;
using SokoGrump.Gui.GuiElements;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        GameEngine game;

        GuiGameBoard gameBoard;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string initialWorldId = "narivia";
            string initialFactionId = "f_caravenna";

            game = new GameEngine();
            game.NewGame(0);

            gameBoard = new GuiGameBoard(game)
            {
                Size = ScreenManager.Instance.Size
            };

            if (ScreenArgs != null && ScreenArgs.Length >= 2)
            {
                initialWorldId = ScreenArgs[0];
                initialFactionId = ScreenArgs[1];
            }

            GuiManager.Instance.GuiElements.Add(gameBoard);

            base.LoadContent();
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

        private void InputManager_KeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            switch(e.Key)
            {
                case Keys.W:
                case Keys.Up:
                    game.MovePlayer(PlayerDirection.North);
                    break;

                case Keys.A:
                case Keys.Left:
                    game.MovePlayer(PlayerDirection.West);
                    break;

                case Keys.S:
                case Keys.Down:
                    game.MovePlayer(PlayerDirection.South);
                    break;

                case Keys.D:
                case Keys.Right:
                    game.MovePlayer(PlayerDirection.East);
                    break;

                case Keys.R:
                    game.Retry();
                    break;
            }
        }
    }
}

