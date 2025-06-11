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
    public class GuiGameBoard(IGameManager game) : GuiControl
    {
        Dictionary<int, TextureSprite> tileSprites;
        TextureSprite targetSprite;
        TextureSprite playerSprite;

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
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new PlayerSpriteSheetEffect(game),
                IsActive = true
            };

            foreach (Tile tile in game.GetTiles())
            {
                TextureSprite tileSprite = new()
                {
                    ContentFile = tile.SpriteSheet,
                    SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                    IsActive = true
                };

                if (tile.Id.Equals(2))
                {
                    tileSprite.SpriteSheetEffect = new CrateSpriteSheetEffect(game);
                }
                else
                {
                    tileSprite.SpriteSheetEffect = new TileSpriteSheetEffect(game);
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

            Player player = game.GetPlayer();

            playerSprite.Location = Location + player.Location * GameDefines.MapTileSize;
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            List<Point2D> targets = game.GetTargets();

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    Tile tile = game.GetTile(x, y);

                    TextureSprite tileSprite = tileSprites[tile.Id];
                    tileSprite.Location = Location + new Point2D(
                        x * GameDefines.MapTileSize,
                        y * GameDefines.MapTileSize);

                    // TODO: This is temporary
                    if (tile.Id.Equals(0) || tile.Id.Equals(1))
                    {
                        TileSpriteSheetEffect tileEffect = (TileSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        tileEffect.TileLocation = new Point2D(x, y);

                        if (tile.Id.Equals(0))
                        {
                            tileEffect.TilesWith = [0, 2, 3, 5];
                        }
                        else if (tile.Id.Equals(1))
                        {
                            tileEffect.TilesWith = [1];
                        }

                        tileEffect.Update(null);
                    }
                    else if (tile.Id.Equals(2))
                    {
                        CrateSpriteSheetEffect crateEffect = (CrateSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        crateEffect.TileLocation = new Point2D(x, y);
                        crateEffect.Update(null);
                    }

                    if (tile.Id.Equals(2) && targets.Any(target => target.X.Equals(x) && target.Y.Equals(y)))
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

            foreach (Point2D targetLocation in targets)
            {
                Tile tile = game.GetTile(targetLocation.X, targetLocation.Y);

                if (tile.Id.Equals(2))
                {
                    continue;
                }

                targetSprite.Location = Location + targetLocation * GameDefines.MapTileSize;

                targetSprite.Draw(spriteBatch);
            }

            playerSprite.Draw(spriteBatch);
        }
    }
}
