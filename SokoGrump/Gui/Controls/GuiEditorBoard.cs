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
            tileSprites = [];
            targetSprite = new TextureSprite
            {
                ContentFile = "SpriteSheets/target"
            };
            playerSprite = new TextureSprite
            {
                ContentFile = "SpriteSheets/player",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize)
            };

            foreach (Tile tile in editor.GetTiles())
            {
                TextureSprite tileSprite = new()
                {
                    ContentFile = tile.SpriteSheet,
                    SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                    IsActive = true
                };

                if (tile.Id.Equals(TileId.CrateOnFloor))
                {
                    tileSprite.SpriteSheetEffect = new BasicTileSpriteSheetEffect();
                }
                else
                {
                    tileSprite.SpriteSheetEffect = new ConnectedTileSpriteSheetEffect(editor);
                }

                tileSprite.LoadContent();
                tileSprite.SpriteSheetEffect.Activate();

                tileSprites.Add((int)tile.Id, tileSprite);
            }

            targetSprite.LoadContent();
            playerSprite.LoadContent();
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

            Player player = editor.GetPlayer();

            playerSprite.Location = Location + player.Location * GameDefines.MapTileSize;
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

                    TextureSprite tileSprite = tileSprites[(int)tile.Id];
                    tileSprite.Location = Location + new Point2D(
                        x * GameDefines.MapTileSize,
                        y * GameDefines.MapTileSize);

                    // TODO: This is temporary
                    if (tile.Id.Equals(TileId.Floor) || tile.Id.Equals(TileId.Wall))
                    {
                        ConnectedTileSpriteSheetEffect tileEffect = (ConnectedTileSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        tileEffect.TileLocation = new Point2D(x, y);

                        if (tile.Id.Equals(TileId.Floor))
                        {
                            tileEffect.TilesWith = [TileId.Floor, TileId.CrateOnFloor, TileId.EmptyTarget, TileId.CrateOnTarget];
                        }
                        else if (tile.Id.Equals(TileId.Wall))
                        {
                            tileEffect.TilesWith = [TileId.Wall];
                        }

                        tileEffect.Update(null);
                    }

                    if (tile.Id.Equals(TileId.CrateOnFloor) && editor.GetTargets().Any(target => target.X == x && target.Y == y))
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

                if (tile.Id.Equals(TileId.CrateOnFloor))
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
