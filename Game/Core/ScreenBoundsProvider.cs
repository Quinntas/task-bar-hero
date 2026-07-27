using System.Drawing;
using Screen = System.Windows.Forms.Screen;

namespace TaskbarHeroOverlay.Game.Core;

public static class ScreenBoundsProvider
{
    public static Rectangle GetPrimaryScreenBounds()
    {
        return Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1280, 720);
    }
}
