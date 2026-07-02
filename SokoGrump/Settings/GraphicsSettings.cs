using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Primitives;

namespace SokoGrump.Settings
{
    public class GraphicsSettings
    {
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

                return new Size2D(1280, 720);
            }
        }

        /// <summary>
        /// Gets or sets the fullscreen mode.
        /// </summary>
        /// <value>The fullscreen mode.</value>
        public bool Fullscreen { get; set; }
    }
}
