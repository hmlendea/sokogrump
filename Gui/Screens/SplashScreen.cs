using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace SokoGrump.Gui.Screens
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    public class SplashScreen : Screen
    {
        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        public float Delay { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>The logo.</value>
        public GuiImage LogoImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen"/> class.
        /// </summary>
        public SplashScreen()
        {
            Delay = 2;
            BackgroundColour = Colour.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            LogoImage = new GuiImage { ContentFile = "SplashScreen/Logo" };

            GuiManager.Instance.RegisterControls(LogoImage);
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
        void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed += OnInputManagerMouseButtonPressed;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed -= OnInputManagerMouseButtonPressed;
        }

        /// <summary>
        /// Sets the properties of the child controls.
        /// </summary>
        void SetChildrenProperties()
            => LogoImage.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - LogoImage.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - LogoImage.Size.Height) / 2);

        void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e) => ChangeScreen();

        void OnInputManagerMouseButtonPressed(object sender, MouseButtonEventArgs e) => ChangeScreen();

        static void ChangeScreen() => ScreenManager.Instance.ChangeScreens<TitleScreen>();
    }
}
