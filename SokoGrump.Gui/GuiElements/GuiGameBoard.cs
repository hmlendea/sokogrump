using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using SokoGrump.GameLogic;
using SokoGrump.Gui.SpriteEffects;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.Gui.GuiElements
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiGameBoard : GuiElement
    {
        /// <summary>
        /// Gets the selected province identifier.
        /// </summary>
        /// <value>The selected province identifier.</value>
        public string SelectedProvinceId { get; private set; }

        GameEngine game;

        TileSpriteSheetEffect tileEffect;
        Dictionary<int, TextureSprite> terrainSprites;
        TextureSprite targetSprite;
        TextureSprite playerSprite;

        public GuiGameBoard(GameEngine game)
        {
            this.game = game;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            terrainSprites = new Dictionary<int, TextureSprite>();
            targetSprite = new TextureSprite
            {
                ContentFile = "SpriteSheets/target"
            };
            playerSprite = new TextureSprite
            {
                ContentFile = "Tiles/player/player",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize)
            };

            tileEffect = new TileSpriteSheetEffect(game);

            foreach (Tile tile in game.GetTiles())
            {
                TextureSprite tileSprite = new TextureSprite
                {
                    ContentFile = tile.SpriteSheet,
                    SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                    SpriteSheetEffect = tileEffect,
                    Active = true
                };

                tileSprite.LoadContent();
                tileSprite.SpriteSheetEffect.Activate();

                terrainSprites.Add(tile.Id, tileSprite);
            }

            targetSprite.LoadContent();
            playerSprite.LoadContent();
            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            terrainSprites.Values.ToList().ForEach(x => x.UnloadContent());
            targetSprite.UnloadContent();
            playerSprite.UnloadContent();
            base.UnloadContent();

            terrainSprites.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            targetSprite.Update(gameTime);
            playerSprite.Update(gameTime);

            Player player = game.GetPlayer();

            playerSprite.Location = new Point2D(
                player.Location.X * GameDefines.MapTileSize,
                player.Location.Y * GameDefines.MapTileSize);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            List<Point2D> targets = game.GetTargets();

            for (int y = 0; y < GameDefines.BoardHeight; y++)
            {
                for (int x = 0; x < GameDefines.BoardWidth; x++)
                {
                    Tile tile = game.GetTile(x, y);

                    TextureSprite terrainSprite = terrainSprites[tile.Id];
                    terrainSprite.Location = new Point2D(x * GameDefines.MapTileSize, y * GameDefines.MapTileSize);

                    // TODO: This is temporary
                    if (tile.Id == 0 || tile.Id == 1)
                    {
                        tileEffect.TileLocation = new Point2D(x, y);

                        if (tile.Id == 0)
                        {
                            tileEffect.TilesWith = new List<int> { 0, 2, 3, 5 };
                        }
                        else if (tile.Id == 1)
                        {
                            tileEffect.TilesWith = new List<int> { 1 };
                        }


                        tileEffect.UpdateFrame(null);

                        terrainSprite.SourceRectangle = new Rectangle2D(
                            tileEffect.CurrentFrame.X * GameDefines.MapTileSize,
                            tileEffect.CurrentFrame.Y * GameDefines.MapTileSize,
                            GameDefines.MapTileSize,
                            GameDefines.MapTileSize);
                    }

                    if (tile.Id == 2 && targets.Any(target => target.X == x && target.Y == y))
                    {
                        terrainSprite.Tint = Colour.Red;
                    }
                    else
                    {
                        terrainSprite.Tint = Colour.White;
                    }

                    terrainSprite.Draw(spriteBatch);
                }
            }

            foreach (Point2D targetLocation in targets)
            {
                Tile tile = game.GetTile(targetLocation.X, targetLocation.Y);

                if (tile.Id == 2)
                {
                    continue;
                }

                targetSprite.Location = new Point2D(
                    targetLocation.X * GameDefines.MapTileSize,
                    targetLocation.Y * GameDefines.MapTileSize);

                targetSprite.Draw(spriteBatch);
            }

            playerSprite.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
