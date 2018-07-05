using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.DataAccess.Resources;

using SokoGrump.Gui.Helpers;
using SokoGrump.Settings;

namespace SokoGrump.Gui
{
    /// <summary>
    /// FPS indicator.
    /// </summary>
    public class FpsIndicator
    {
        GameTime gameTime;
        SpriteFont fpsFont;
        Vector2 fpsCounterSize;
        string fpsString;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Vector2 Location { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FpsIndicator"/> class.
        /// </summary>
        public FpsIndicator()
        {
            Location = Vector2.Zero;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            fpsFont = ResourceManager.Instance.LoadSpriteFont("Fonts/FrameCounterFont");
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {

        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            fpsString = $"FPS: {Math.Round(FramerateCounter.Instance.AverageFramesPerSecond)}";
            fpsCounterSize = fpsFont.MeasureString(fpsString);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FramerateCounter.Instance.Update(deltaTime);

            if (SettingsManager.Instance.DebugMode)
            {
                spriteBatch.DrawString(fpsFont, fpsString, Vector2.One, Color.Lime);
            }
        }
    }
}
