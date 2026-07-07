using System.Collections.Generic;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Models;
using SokoGrump.Settings;

namespace SokoGrump.Gui.SpriteEffects
{
    public class ConnectedTileSpriteSheetEffect : SpriteSheetEffect
    {
        readonly IGameManager game;
        readonly IEditorManager editor;

        public Point2D TileLocation { get; set; }

        public List<TileId> TilesWith { get; set; }

        public ConnectedTileSpriteSheetEffect(IGameManager game)
            : base()
        {
            FrameAmount = new Size2D(3, 6);
            TilesWith = [];

            this.game = game;
        }

        public ConnectedTileSpriteSheetEffect(IEditorManager editor)
            : base()
        {
            FrameAmount = new Size2D(3, 6);
            TilesWith = [];

            this.editor = editor;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            // TODO: Dirty fix
            if (TileLocation.X == 0 || TileLocation.X == GameDefines.BoardWidth - 1 ||
                TileLocation.Y == 0 || TileLocation.Y == GameDefines.BoardHeight - 1)
            {
                return;
            }

            TileId id = GetTileId(TileLocation.X, TileLocation.Y);
            TileId idN = GetTileId(TileLocation.X, TileLocation.Y - 1);
            TileId idW = GetTileId(TileLocation.X - 1, TileLocation.Y);
            TileId idS = GetTileId(TileLocation.X, TileLocation.Y + 1);
            TileId idE = GetTileId(TileLocation.X + 1, TileLocation.Y);

            bool tilesN = TilesWith.Contains(idN);
            bool tilesW = TilesWith.Contains(idW);
            bool tilesS = TilesWith.Contains(idS);
            bool tilesE = TilesWith.Contains(idE);

            if (tilesN && tilesW && tilesS && tilesE) // Middle
            {
                CurrentFrame = new Point2D(1, 4);
            }
            else if (!tilesN && !tilesW && !tilesS && !tilesE) // Single
            {
                CurrentFrame = new Point2D(0, 0);
            }
            else if (!tilesN && !tilesW && tilesS && tilesE) // TopLeftCorner
            {
                CurrentFrame = new Point2D(0, 3);
            }
            else if (!tilesN && tilesW && tilesS && !tilesE) // TopRightCorner
            {
                CurrentFrame = new Point2D(2, 3);
            }
            else if (tilesN && !tilesW && !tilesS && tilesE) // BottomLeftCorner
            {
                CurrentFrame = new Point2D(0, 5);
            }
            else if (tilesN && tilesW && !tilesS && !tilesE) // BottomRightCorner
            {
                CurrentFrame = new Point2D(2, 5);
            }
            else if (!tilesN && tilesW && tilesS && tilesE) // TopCorner
            {
                CurrentFrame = new Point2D(1, 3);
            }
            else if (tilesN && !tilesW && tilesS && tilesE) // LeftCorner
            {
                CurrentFrame = new Point2D(0, 4);
            }
            else if (tilesN && tilesW && !tilesS && tilesE) // BottomCorner
            {
                CurrentFrame = new Point2D(1, 5);
            }
            else if (tilesN && tilesW && tilesS && !tilesE) // RightCorner
            {
                CurrentFrame = new Point2D(2, 4);
            }
            else if (!tilesN && !tilesW && tilesS && !tilesE) // ^
            {
                CurrentFrame = new Point2D(0, 1);
            }
            else if (tilesN && !tilesW && tilesS && !tilesE) // |
            {
                CurrentFrame = new Point2D(1, 1);
            }
            else if (tilesN && !tilesW && !tilesS && !tilesE) // v
            {
                CurrentFrame = new Point2D(2, 1);
            }
            else if (!tilesN && !tilesW && !tilesS && tilesE) // <
            {
                CurrentFrame = new Point2D(0, 2);
            }
            else if (!tilesN && tilesW && !tilesS && tilesE) // -
            {
                CurrentFrame = new Point2D(1, 2);
            }
            else if (!tilesN && tilesW && !tilesS && !tilesE) // >
            {
                CurrentFrame = new Point2D(2, 2);
            }
            else
            {
                CurrentFrame = new Point2D(0, 0);
            }
        }

        private TileId GetTileId(int x, int y)
        {
            if (game is not null)
            {
                return game.GetTile(x, y).Id;
            }

            if (editor is not null)
            {
                return editor.GetTile(x, y).Id;
            }

            return TileId.None;
        }
    }
}
