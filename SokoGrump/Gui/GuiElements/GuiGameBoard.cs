using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using SokoGrump.GameLogic;
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

        Dictionary<int, TextureSprite> terrainSprites;
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
            playerSprite = new TextureSprite
            {
                ContentFile = "Tiles/player/player",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize)
            };

            for (int i = 0; i <= 7; i++)
            {
                // TODO: This is just temporary so that it can fit the old system
                if (i == 4 || i == 6)
                {
                    continue;
                }

                TextureSprite tileSprite = new TextureSprite
                {
                    ContentFile = $"Tiles/tile{i}/0",
                    SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize)
                };

                tileSprite.LoadContent();
                terrainSprites.Add(i, tileSprite);
            }

            playerSprite.LoadContent();
            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            terrainSprites.Values.ToList().ForEach(x => x.UnloadContent());
            playerSprite.UnloadContent();

            terrainSprites.Clear();

            base.UnloadContent();
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < game.TableHeight; y++)
            {
                for (int x = 0; x < game.TableWidth; x++)
                {
                    Tile tile = game.tiles[x, y];

                    TextureSprite terrainSprite = terrainSprites[tile.ID];
                    terrainSprite.Location = new Point2D(x * GameDefines.MapTileSize, y * GameDefines.MapTileSize);
                    terrainSprite.Draw(spriteBatch);
                }
            }

            playerSprite.Location = new Point2D(
                game.PlayerPosX * GameDefines.MapTileSize,
                game.PlayerPosY * GameDefines.MapTileSize);
            playerSprite.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
