using Microsoft.Xna.Framework;
using NuciXNA.Gui;
using NuciXNA.Gui.GuiElements;
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
        public override void LoadContent()
        {
            LogoImage = new GuiImage { ContentFile = "SplashScreen/Logo" };

            GuiManager.Instance.GuiElements.Add(LogoImage);

            base.LoadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Delay <= 0)
            {
                ChangeScreen();
            }

            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void SetChildrenProperties()
        {
            LogoImage.Location = new Point2D((ScreenManager.Instance.Size.Width - LogoImage.Size.Width) / 2,
                                             (ScreenManager.Instance.Size.Height - LogoImage.Size.Height) / 2);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            InputManager.Instance.KeyboardKeyPressed += InputManager_KeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed += InputManager_MouseButtonPressed;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            InputManager.Instance.KeyboardKeyPressed -= InputManager_KeyboardKeyPressed;
            InputManager.Instance.MouseButtonPressed -= InputManager_MouseButtonPressed;
        }

        void InputManager_KeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            ChangeScreen();
        }

        void InputManager_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ChangeScreen();
        }

        void ChangeScreen()
        {
            ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen), default(int));
        }
    }
}
