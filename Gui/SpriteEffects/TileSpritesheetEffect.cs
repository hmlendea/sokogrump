using System.Collections.Generic;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

using SokoGrump.GameLogic.GameManagers;
using SokoGrump.Settings;

namespace SokoGrump.Gui.SpriteEffects
{
    public class TileSpriteSheetEffect : SpriteSheetEffect
    {
        // Bitmask: N=8, W=4, S=2, E=1
        static readonly Dictionary<int, Point2D> FrameMap = new()
        {
            { 0b0000, new Point2D(0, 0) }, // Single
            { 0b0001, new Point2D(0, 2) }, // <
            { 0b0010, new Point2D(0, 1) }, // ^
            { 0b0011, new Point2D(0, 3) }, // TopLeftCorner
            { 0b0100, new Point2D(2, 2) }, // >
            { 0b0101, new Point2D(1, 2) }, // -
            { 0b0110, new Point2D(2, 3) }, // TopRightCorner
            { 0b0111, new Point2D(1, 3) }, // TopCorner
            { 0b1000, new Point2D(2, 1) }, // v
            { 0b1001, new Point2D(0, 5) }, // BottomLeftCorner
            { 0b1010, new Point2D(1, 1) }, // |
            { 0b1011, new Point2D(0, 4) }, // LeftCorner
            { 0b1100, new Point2D(2, 5) }, // BottomRightCorner
            { 0b1101, new Point2D(1, 5) }, // BottomCorner
            { 0b1110, new Point2D(2, 4) }, // RightCorner
            { 0b1111, new Point2D(1, 4) }, // Middle
        };
        readonly IGameManager game;

        public Point2D TileLocation { get; set; }

        public List<int> TilesWith { get; set; }

        public TileSpriteSheetEffect(IGameManager game) : base()
        {
            FrameAmount = new Size2D(3, 6);
            TilesWith = [];

            this.game = game;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            // TODO: Dirty fix
            if (TileLocation.X.Equals(0) || TileLocation.X.Equals(GameDefines.BoardWidth - 1) ||
                TileLocation.Y.Equals(0) || TileLocation.Y.Equals(GameDefines.BoardHeight - 1))
            {
                return;
            }

            int idN = game.GetTile(TileLocation.X, TileLocation.Y - 1).Id;
            int idW = game.GetTile(TileLocation.X - 1, TileLocation.Y).Id;
            int idS = game.GetTile(TileLocation.X, TileLocation.Y + 1).Id;
            int idE = game.GetTile(TileLocation.X + 1, TileLocation.Y).Id;

            bool tilesN = TilesWith.Contains(idN);
            bool tilesW = TilesWith.Contains(idW);
            bool tilesS = TilesWith.Contains(idS);
            bool tilesE = TilesWith.Contains(idE);

            int mask =
                (tilesN ? 8 : 0) |
                (tilesW ? 4 : 0) |
                (tilesS ? 2 : 0) |
                (tilesE ? 1 : 0);

            CurrentFrame = FrameMap[mask];
        }
    }
}
