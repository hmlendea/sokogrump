using Microsoft.Xna.Framework;
using NuciXNA.Gui;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;
using NuciXNA.Input.Events;
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
            Delay = 3;
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

            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void SetChildrenProperties()
        {
            LogoImage.Location = new Point2D((ScreenManager.Instance.Size.Width - LogoImage.Size.Width) / 2,
                                             (ScreenManager.Instance.Size.Height - LogoImage.Size.Height) / 2);
        }

        protected override void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            base.OnKeyPressed(sender, e);

            ScreenManager.Instance.ChangeScreens(typeof(SplashScreen));
        }

        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseButtonPressed(sender, e);

            ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen));
        }
    }
}
