using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.DataAccess.Content;
using NuciXNA.Graphics;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

using SokoGrump.Gui;
using SokoGrump.Gui.Screens;
using SokoGrump.Localisation;
using SokoGrump.Settings;

namespace SokoGrump
{
    /// <summary>
    /// This is the main type for the game.
    /// </summary>
    public class GameWindow : Game
    {
        readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        readonly FpsIndicator fpsIndicator;
        readonly Cursor cursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };
            Content.RootDirectory = "Content";

            fpsIndicator = new FpsIndicator();
            cursor = new Cursor
            {
                SpriteSize = new Size2D(64, 80),
                Scale = new Scale2D(0.5f, 0.5f)
            };
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsManager.Instance.SpriteBatch = spriteBatch;
            GraphicsManager.Instance.Graphics = graphics;

            NuciContentManager.Instance.LoadContent(Content, GraphicsDevice);
            SettingsManager.Instance.LoadContent();
            LocalisationManager.Instance.LoadContent();

            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.StartingScreenType = typeof(SplashScreen);
            ScreenManager.Instance.LoadContent();

            fpsIndicator.LoadContent();
            cursor.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();

            FpsIndicator.UnloadContent();
            cursor.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SettingsManager.Instance.Update();
            ScreenManager.Instance.Update(gameTime);

            if (IsActive)
            {
                InputManager.Instance.Update(Window);
            }
            else
            {
                // TODO: It shouldn't reset them every single tick when the window is not active.
                InputManager.Instance.ResetInputStates();
            }

            fpsIndicator.Update(gameTime);
            cursor.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp);

            ScreenManager.Instance.Draw(spriteBatch);

            fpsIndicator.Draw(spriteBatch);
            cursor.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
