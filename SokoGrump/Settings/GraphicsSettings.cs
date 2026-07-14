using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Primitives;

namespace SokoGrump.Settings
{
    public sealed class GraphicsSettings
    {
        private static int DefaultResolutionWidth => 1280;

        private static int DefaultResolutionHeight => 720;

        /// <summary>
        /// Gets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public Size2D Resolution
        {
            get
            {
                if (Fullscreen)
                {
                    return new Size2D(
                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                }

                return new Size2D(DefaultResolutionWidth, DefaultResolutionHeight);
            }
        }

        /// <summary>
        /// Gets or sets the fullscreen mode.
        /// </summary>
        /// <value>The fullscreen mode.</value>
        public bool Fullscreen { get; set; }
    }
}
