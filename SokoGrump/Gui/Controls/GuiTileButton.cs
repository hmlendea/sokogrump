using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;

namespace SokoGrump.Gui.Controls
{
    /// <summary>
    /// Button GUI element.
    /// </summary>
    public class GuiTileButton : GuiControl, IGuiControl
    {
        public int TileId { get; }

        GuiImage image;

        public GuiTileButton(int tileId)
        {
            TileId = tileId;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            image = new GuiImage
            {
                Id = $"{Id}_{nameof(image)}",
                ContentFile = $"Tiles/tile{TileId}/0"
            };
            
            RegisterChildren(image);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {

        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }
    }
}
