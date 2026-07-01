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
        Dictionary<TileId, TextureSprite> tileSprites;
        TextureSprite targetSprite;
        GuiImage playerAvatar;
        GuiImage pushedBox;

        Point2D pushedBoxStartTile;
        bool isPushingBox;
        bool pushedBoxWasOnTarget;
        bool isUndoAnimation;

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
            playerAvatar = new GuiImage
            {
                ContentFile = "SpriteSheets/player",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new PlayerSpriteSheetEffect(game),
                MovementEffect = new MovementEffect(),
                AreEffectsActive = true
            };

            foreach (Tile tile in game.GetTiles())
            {
                TextureSprite tileSprite = new()
                {
                    ContentFile = tile.SpriteSheet,
                    SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                    IsActive = true
                };

                if (tile.Id.Equals(TileId.CrateOnGround))
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
            playerAvatar.LoadContent();
            playerAvatar.SpriteSheetEffect.Activate();

            pushedBox = new GuiImage
            {
                ContentFile = tileSprites[TileId.CrateOnGround].ContentFile,
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.MapTileSize, GameDefines.MapTileSize),
                SpriteSheetEffect = new CrateSpriteSheetEffect(game),
                MovementEffect = new MovementEffect(),
                AreEffectsActive = true
            };
            pushedBox.LoadContent();
            pushedBox.SpriteSheetEffect.Activate();

            playerAvatar.MovementEffect.Deactivated += OnPlayerSpriteMovementEffectDeactivated;
            pushedBox.MovementEffect.Deactivated += OnPushedBoxMovementEffectDeactivated;
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            tileSprites.Values.ToList().ForEach(x => x.UnloadContent());
            targetSprite.UnloadContent();
            playerAvatar.UnloadContent();
            pushedBox.UnloadContent();

            tileSprites.Clear();

            playerAvatar.MovementEffect.Deactivated -= OnPlayerSpriteMovementEffectDeactivated;
            pushedBox.MovementEffect.Deactivated -= OnPushedBoxMovementEffectDeactivated;
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            targetSprite.Update(gameTime);
            this.playerAvatar.Update(gameTime);

            if (isPushingBox)
            {
                pushedBox.Update(gameTime);
            }

            Player player = game.GetPlayer();

            this.playerAvatar.Location = Location + player.Location * GameDefines.MapTileSize;
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
                        TextureSprite groundSprite = tileSprites[TileId.Ground];
                        groundSprite.Location = Location + new Point2D(x * GameDefines.MapTileSize, y * GameDefines.MapTileSize);
                        TileSpriteSheetEffect groundEffect = (TileSpriteSheetEffect)groundSprite.SpriteSheetEffect;
                        groundEffect.TileLocation = new Point2D(x, y);
                        groundEffect.TilesWith = [TileId.Ground, TileId.CrateOnGround, TileId.EmptyTarget, TileId.CrateOnTarget];
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
                    if (tile.Id.Equals(TileId.Ground) || tile.Id.Equals(TileId.Wall))
                    {
                        TileSpriteSheetEffect tileEffect = (TileSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        tileEffect.TileLocation = new Point2D(x, y);

                        if (tile.Id.Equals(TileId.Ground))
                        {
                            tileEffect.TilesWith = [TileId.Ground, TileId.CrateOnGround, TileId.EmptyTarget, TileId.CrateOnTarget];
                        }
                        else if (tile.Id.Equals(TileId.Wall))
                        {
                            tileEffect.TilesWith = [TileId.Wall];
                        }

                        tileEffect.Update(null);
                    }
                    else if (tile.Id.Equals(TileId.CrateOnGround))
                    {
                        CrateSpriteSheetEffect crateEffect = (CrateSpriteSheetEffect)tileSprite.SpriteSheetEffect;

                        crateEffect.TileLocation = new Point2D(x, y);
                        crateEffect.Update(null);
                    }

                    if (tile.Id.Equals(TileId.CrateOnGround) && targets.Any(target => target.X.Equals(x) && target.Y.Equals(y)))
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

                if (tile.Id.Equals(TileId.CrateOnGround) && !isAnimatedCratePos)
                {
                    continue;
                }

                targetSprite.Location = Location + targetLocation * GameDefines.MapTileSize;

                targetSprite.Draw(spriteBatch);
            }

            playerAvatar.Draw(spriteBatch);

            if (isPushingBox)
            {
                pushedBox.TintColour = pushedBoxWasOnTarget ? Colour.Red : Colour.White;
                pushedBox.Draw(spriteBatch);
            }
        }

        void MovePlayer(MovementDirection direction)
        {
            if (this.playerAvatar.MovementEffect.IsActive)
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

                CrateSpriteSheetEffect crateEffect = (CrateSpriteSheetEffect)pushedBox.SpriteSheetEffect;
                crateEffect.TileLocation = pushedBoxStartTile;
                crateEffect.Update(null);

                pushedBox.Location = boxPixelStart;
                pushedBox.MovementEffect.TargetLocation = boxPixelStart + new Point2D(dirX * GameDefines.MapTileSize, dirY * GameDefines.MapTileSize);
                pushedBox.MovementEffect.Activate();
            }

            Point2D targetLocation = this.playerAvatar.Location;

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

            this.playerAvatar.MovementEffect.TargetLocation = targetLocation;
            this.playerAvatar.MovementEffect.Activate();
        }

        public void UndoPlayer()
        {
            if (playerAvatar.MovementEffect.IsActive || !game.CanUndo)
            {
                return;
            }

            UndoInfo undoInfo = game.PeekUndo();

            if (undoInfo.CratePushed)
            {
                pushedBoxStartTile = undoInfo.CrateAnimStart;
                isPushingBox = true;
                pushedBoxWasOnTarget = game.GetTargets().Any(t => t.X == undoInfo.CrateAnimStart.X && t.Y == undoInfo.CrateAnimStart.Y);

                Point2D cratePixelStart = Location + undoInfo.CrateAnimStart * GameDefines.MapTileSize;
                Point2D cratePixelEnd = Location + undoInfo.CrateAnimEnd * GameDefines.MapTileSize;

                CrateSpriteSheetEffect crateEffect = (CrateSpriteSheetEffect)pushedBox.SpriteSheetEffect;
                crateEffect.TileLocation = undoInfo.CrateAnimStart;
                crateEffect.Update(null);

                pushedBox.Location = cratePixelStart;
                pushedBox.MovementEffect.TargetLocation = cratePixelEnd;
                pushedBox.MovementEffect.Activate();
            }

            Point2D playerPixelTarget = Location + undoInfo.PlayerTarget * GameDefines.MapTileSize;
            playerAvatar.MovementEffect.TargetLocation = playerPixelTarget;
            playerAvatar.MovementEffect.Activate();

            isUndoAnimation = true;
        }

        void OnPushedBoxMovementEffectDeactivated(object sender, EventArgs e)
            => pushedBox.Location = pushedBox.MovementEffect.TargetLocation;

        void OnPlayerSpriteMovementEffectDeactivated(object sender, EventArgs e)
        {
            isPushingBox = false;

            Player player = game.GetPlayer();

            if (isUndoAnimation)
            {
                isUndoAnimation = false;
                game.Undo();
            }
            else
            {
                game.MovePlayer(player.Direction);
            }

            player = game.GetPlayer();
            playerAvatar.Location = Location + player.Location * GameDefines.MapTileSize;
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
                    UndoPlayer();
                    break;
            }
        }
    }
}
