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
    public sealed class GuiButton : GuiControl, IGuiControl
    {
        public string Text { get; set; }

        public string TooltipText { get; set; }

        public string ContentFile { get; set; }

        private GuiImage image;
        private GuiTooltip tooltip;
        private int frameSize;
        private bool hasPressedFrame;

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

        private void RegisterEvents()
        {
            MouseEntered += OnMouseEntered;
            MouseLeft += OnMouseLeft;
        }

        private void UnregisterEvents()
        {
            MouseEntered -= OnMouseEntered;
            MouseLeft -= OnMouseLeft;
        }

        private void SetChildrenProperties()
        {
            if (frameSize == 0 && image.SourceRectangle.Height > 0 &&
                image.SourceRectangle.Width > image.SourceRectangle.Height)
            {
                frameSize = image.SourceRectangle.Height;
                hasPressedFrame = image.SourceRectangle.Width >= frameSize * 3;
            }

            if (frameSize > 0)
            {
                bool isPressed = IsHovered && InputManager.Instance.IsMouseButtonDown(MouseButton.Left);

                int frameIndex = 0;

                if (hasPressedFrame && isPressed)
                {
                    frameIndex = 2;
                }
                else if (IsHovered)
                {
                    frameIndex = 1;
                }

                image.SourceRectangle = new Rectangle2D(frameIndex * frameSize, 0, frameSize, frameSize);
            }

            tooltip.Text = TooltipText;
            tooltip.Location = new Point2D(0, Size.Height);
        }

        private void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tooltip.Text))
            {
                tooltip.Show();
            }
        }

        private void OnMouseLeft(object sender, MouseEventArgs e) => tooltip.Hide();
    }
}
