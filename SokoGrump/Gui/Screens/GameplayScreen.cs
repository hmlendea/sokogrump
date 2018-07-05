using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
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
    }
}

