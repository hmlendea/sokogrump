using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Gui.SpriteEffects;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.Gui.Controls
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiEditorBoard : GuiControl
    {
        IEditorManager editor;

        Dictionary<int, TextureSprite> tileSprites;
        TextureSprite targetSprite;
        TextureSprite playerSprite;

        public GuiEditorBoard(IEditorManager editor)
        {
            this.editor = editor;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            tileSprites = new Dictionary<int, TextureSprite>();
            targetSprite = new TextureSprite
            {
                ContentFile = "SpriteSheets/target"
            };
            playerSprite = new TextureSprite
            {
                ContentFile = "SpriteSheets/player",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new PlayerSpriteSheetEffect(),
                IsActive = true
            };

            foreach (Tile tile in editor.GetTiles())
            {
                TextureSprite tileSprite = new TextureSprite
                {
                    ContentFile = tile.SpriteSheet,
                    SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                    IsActive = true
                };

                if (tile.Id == 2)
                {
                    tileSprite.SpriteSheetEffect = new BasicTileSpriteSheetEffect();
                }
                else
                {
                    tileSprite.SpriteSheetEffect = new ConnectedTileSpriteSheetEffect(editor);
                }

                tileSprite.LoadContent();
                tileSprite.SpriteSheetEffect.Activate();

                tileSprites.Add(tile.Id, tileSprite);
            }

            targetSprite.LoadContent();
            playerSprite.LoadContent();
            playerSprite.SpriteSheetEffect.Activate();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            tileSprites.Values.ToList().ForEach(x => x.UnloadContent());
            targetSprite.UnloadContent();
            playerSprite.UnloadContent();

            tileSprites.Clear();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            targetSprite.Update(gameTime);
            playerSprite.Update(gameTime);

            //Player player = board.GetPlayer();

            //playerSprite.Location = Location + player.Location * GameDefines.MapTileSize;
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    Tile tile = editor.GetTile(x, y);

                    TextureSprite tileSprite = tileSprites[tile.Id];
                    tileSprite.Location = Location + new Point2D(
                        x * GameDefines.MapTileSize,
                        y * GameDefines.MapTileSize);

                    // TODO: This is temporary
                    if (tile.Id == 0 || tile.Id == 1)
                    {
                        ConnectedTileSpriteSheetEffect tileEffect = (ConnectedTileSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        tileEffect.TileLocation = new Point2D(x, y);

                        if (tile.Id == 0)
                        {
                            tileEffect.TilesWith = new List<int> { 0, 2, 3, 5 };
                        }
                        else if (tile.Id == 1)
                        {
                            tileEffect.TilesWith = new List<int> { 1 };
                        }

                        tileEffect.Update(null);
                    }

                    if (tile.Id == 2 && editor.GetTargets().Any(target => target.X == x && target.Y == y))
                    {
                        tileSprite.Tint = Colour.Red;
                    }
                    else
                    {
                        tileSprite.Tint = Colour.White;
                    }

                    tileSprite.Draw(spriteBatch);
                }
            }

            foreach (Point2D targetLocation in editor.GetTargets())
            {
                Tile tile = editor.GetTile(targetLocation.X, targetLocation.Y);

                if (tile.Id == 2)
                {
                    continue;
                }

                targetSprite.Location = new Point2D(
                    Location.X + targetLocation.X * GameDefines.MapTileSize,
                    Location.Y + targetLocation.Y * GameDefines.MapTileSize);

                targetSprite.Draw(spriteBatch);
            }

            playerSprite.Draw(spriteBatch);
        }
    }
}
