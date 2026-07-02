using NuciXNA.Primitives;

namespace SokoGrump.GameLogic.GameManagers
{
    public readonly record struct UndoInfo(
        Point2D PlayerTarget,
        bool CratePushed,
        Point2D CrateAnimStart,
        Point2D CrateAnimEnd);
}
