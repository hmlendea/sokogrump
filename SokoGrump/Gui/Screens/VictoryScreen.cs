using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Victory screen.
    /// </summary>
    public sealed class VictoryScreen : Screen
    {
        private static float InitialDelay => 1;
        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        public float Delay { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public GuiImage Image { get; set; }

        private readonly int level;

        /// <summary>
        /// Initializes a new instance of the <see cref="VictoryScreen"/> class.
        /// </summary>
        public VictoryScreen(int level)
        {
            this.level = level;

            Delay = InitialDelay;
            BackgroundColour = Colour.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            Image = new GuiImage
            {
                ContentFile = "Images/grumpy_cat_good",
                TextureLayout = TextureLayout.Stretch
            };

            GuiManager.Instance.RegisterControls(Image);
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent() => UnregisterEvents();

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            if (Delay <= 0)
            {
                ChangeScreen();
            }

            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

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
        private void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed += OnInputManagerMouseButtonPressed;
        }

        private void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed -= OnInputManagerMouseButtonPressed;
        }

        private void SetChildrenProperties() => Image.Size = ScreenManager.Instance.Size;

        private void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e) => ChangeScreen();

        private void OnInputManagerMouseButtonPressed(object sender, MouseButtonEventArgs e) => ChangeScreen();

        private void ChangeScreen() => ScreenManager.Instance.ChangeScreens<GameplayScreen>(level);
    }
}
