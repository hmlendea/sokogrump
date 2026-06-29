using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
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
        TextureSprite pushedBoxAnimSprite;

        Point2D pushedBoxStartTile;
        bool isPushingBox;
        bool pushedBoxWasOnTarget;

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
                MovementEffect = new MovementEffect(),
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

                if (tile.Id.Equals((int)TileId.CrateOnGround))
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

            pushedBoxAnimSprite = new TextureSprite
            {
                ContentFile = tileSprites[(int)TileId.CrateOnGround].ContentFile,
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new CrateSpriteSheetEffect(game),
                MovementEffect = new MovementEffect(),
                IsActive = true
            };
            pushedBoxAnimSprite.LoadContent();
            pushedBoxAnimSprite.SpriteSheetEffect.Activate();

            playerSprite.MovementEffect.Deactivated += OnPlayerSpriteMovementEffectDeactivated;
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            tileSprites.Values.ToList().ForEach(x => x.UnloadContent());
            targetSprite.UnloadContent();
            playerSprite.UnloadContent();
            pushedBoxAnimSprite.UnloadContent();

            tileSprites.Clear();

            playerSprite.MovementEffect.Deactivated -= OnPlayerSpriteMovementEffectDeactivated;
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            targetSprite.Update(gameTime);
            playerSprite.Update(gameTime);

            if (isPushingBox)
            {
                pushedBoxAnimSprite.Update(gameTime);
            }

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
                    if (isPushingBox && x == pushedBoxStartTile.X && y == pushedBoxStartTile.Y)
                    {
                        TextureSprite groundSprite = tileSprites[(int)TileId.Ground];
                        groundSprite.Location = Location + new Point2D(x * GameDefines.MapTileSize, y * GameDefines.MapTileSize);
                        TileSpriteSheetEffect groundEffect = (TileSpriteSheetEffect)groundSprite.SpriteSheetEffect;
                        groundEffect.TileLocation = new Point2D(x, y);
                        groundEffect.TilesWith = [0, 2, 3, 5];
                        groundEffect.Update(null);
                        groundSprite.Tint = Colour.White;
                        groundSprite.Draw(spriteBatch);
                        continue;
                    }

                    Tile tile = game.GetTile(x, y);

                    TextureSprite tileSprite = tileSprites[tile.Id];
                    tileSprite.Location = Location + new Point2D(
                        x * GameDefines.MapTileSize,
                        y * GameDefines.MapTileSize);

                    // TODO: This is temporary
                    if (tile.Id.Equals((int)TileId.Ground) || tile.Id.Equals((int)TileId.Wall))
                    {
                        TileSpriteSheetEffect tileEffect = (TileSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        tileEffect.TileLocation = new Point2D(x, y);

                        if (tile.Id.Equals((int)TileId.Ground))
                        {
                            tileEffect.TilesWith = [0, 2, 3, 5];
                        }
                        else if (tile.Id.Equals((int)TileId.Wall))
                        {
                            tileEffect.TilesWith = [1];
                        }

                        tileEffect.Update(null);
                    }
                    else if (tile.Id.Equals((int)TileId.CrateOnGround))
                    {
                        CrateSpriteSheetEffect crateEffect = (CrateSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        crateEffect.TileLocation = new Point2D(x, y);
                        crateEffect.Update(null);
                    }

                    if (tile.Id.Equals((int)TileId.CrateOnGround) && targets.Any(target => target.X.Equals(x) && target.Y.Equals(y)))
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

                bool isAnimatedCratePos = isPushingBox
                    && targetLocation.X == pushedBoxStartTile.X
                    && targetLocation.Y == pushedBoxStartTile.Y;

                if (tile.Id.Equals((int)TileId.CrateOnGround) && !isAnimatedCratePos)
                {
                    continue;
                }

                targetSprite.Location = Location + targetLocation * GameDefines.MapTileSize;

                targetSprite.Draw(spriteBatch);
            }

            playerSprite.Draw(spriteBatch);

            if (isPushingBox)
            {
                pushedBoxAnimSprite.Tint = pushedBoxWasOnTarget ? Colour.Red : Colour.White;
                pushedBoxAnimSprite.Draw(spriteBatch);
            }
        }

        void MovePlayer(MovementDirection direction)
        {
            if (playerSprite.MovementEffect.IsActive)
            {
                return;
            }

            game.SetPlayerDirection(direction);

            if (!game.CanMove(direction))
            {
                return;
            }

            Player player = game.GetPlayer();
            int dirX = direction switch { MovementDirection.West => -1, MovementDirection.East => 1, _ => 0 };
            int dirY = direction switch { MovementDirection.North => -1, MovementDirection.South => 1, _ => 0 };
            int destX = player.Location.X + dirX;
            int destY = player.Location.Y + dirY;

            if (game.GetTile(destX, destY).TileType is TileType.Moveable)
            {
                pushedBoxStartTile = new Point2D(destX, destY);
                isPushingBox = true;
                pushedBoxWasOnTarget = game.GetTargets().Any(t => t.X == destX && t.Y == destY);

                Point2D boxPixelStart = Location + pushedBoxStartTile * GameDefines.MapTileSize;

                CrateSpriteSheetEffect crateEffect = (CrateSpriteSheetEffect)pushedBoxAnimSprite.SpriteSheetEffect;
                crateEffect.TileLocation = pushedBoxStartTile;
                crateEffect.Update(null);

                pushedBoxAnimSprite.Location = boxPixelStart;
                pushedBoxAnimSprite.MovementEffect.TargetLocation = boxPixelStart + new Point2D(dirX * GameDefines.MapTileSize, dirY * GameDefines.MapTileSize);
                pushedBoxAnimSprite.MovementEffect.Activate();
            }

            Point2D targetLocation = playerSprite.Location;

            if (direction is MovementDirection.North)
            {
                targetLocation.Y -= GameDefines.MapTileSize;
            }
            else if (direction is MovementDirection.West)
            {
                targetLocation.X -= GameDefines.MapTileSize;
            }
            else if (direction is MovementDirection.South)
            {
                targetLocation.Y += GameDefines.MapTileSize;
            }
            else if (direction is MovementDirection.East)
            {
                targetLocation.X += GameDefines.MapTileSize;
            }

            playerSprite.MovementEffect.TargetLocation = targetLocation;
            playerSprite.MovementEffect.Activate();
        }

        void OnPlayerSpriteMovementEffectDeactivated(object sender, EventArgs e)
        {
            isPushingBox = false;

            Player player = game.GetPlayer();
            game.MovePlayer(player.Direction);
            playerSprite.Location = Location + player.Location * GameDefines.MapTileSize;
        }

        void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                case Keys.Up:
                    MovePlayer(MovementDirection.North);
                    break;

                case Keys.A:
                case Keys.Left:
                    MovePlayer(MovementDirection.West);
                    break;

                case Keys.S:
                case Keys.Down:
                    MovePlayer(MovementDirection.South);
                    break;

                case Keys.D:
                case Keys.Right:
                    MovePlayer(MovementDirection.East);
                    break;

                case Keys.R:
                    isPushingBox = false;
                    game.Retry();
                    break;

                case Keys.U:
                    isPushingBox = false;
                    game.Undo();
                    break;
            }
        }
    }
}
