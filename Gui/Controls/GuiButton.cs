using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace SokoGrump.Gui.Controls
{
    /// <summary>
    /// Button GUI element.
    /// </summary>
    public class GuiButton : GuiControl, IGuiControl
    {
        public string Text { get; set; }

        public string TooltipText { get; set; }

        public string ContentFile { get; set; }

        GuiImage image;
        GuiTooltip tooltip;

        public GuiButton() => FontName = "ButtonFont";

        protected override void DoLoadContent()
        {
            image = new GuiImage
            {
                Id = $"{Id}_{nameof(image)}",
                ContentFile = ContentFile
            };
            tooltip = new GuiTooltip()
            {
                FontName = "ToolTipFont",
                Size = new Size2D((int)(Size.Width * 2.5), (int)(Size.Height * 0.8))
            };

            RegisterChildren(image, tooltip);
            RegisterEvents();
            SetChildrenProperties();
        }

        protected override void DoUnloadContent() => UnregisterEvents();

        protected override void DoUpdate(GameTime gameTime) => SetChildrenProperties();

        protected override void DoDraw(SpriteBatch spriteBatch) { }

        void RegisterEvents()
        {
            MouseEntered += OnMouseEntered;
            MouseLeft += OnMouseLeft;
        }

        void UnregisterEvents()
        {
            MouseEntered -= OnMouseEntered;
            MouseLeft -= OnMouseLeft;
        }

        void SetChildrenProperties()
        {
            image.ContentFile = image.ContentFile;
            tooltip.Text = TooltipText;
            tooltip.Location = new Point2D(0, Size.Height);
        }

        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tooltip.Text))
            {
                tooltip.Show();
            }
        }

        void OnMouseLeft(object sender, MouseEventArgs e) => tooltip.Hide();
    }
}
