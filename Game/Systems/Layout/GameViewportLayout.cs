using System;
using System.Drawing;
using TaskbarHeroOverlay.Game.Core;
using TaskbarHeroOverlay.Game.Scenes.DesktopOverlay;

namespace TaskbarHeroOverlay.Game.Systems.Layout;

public static class GameViewportLayout
{
    public static GameViewport ClampToBounds(double left, double top, double width, double height, Rectangle bounds)
    {
        var clampedWidth = Math.Max(DesktopOverlaySceneConfig.MinimumWidth, Math.Min(width, bounds.Width));
        var clampedHeight = Math.Max(DesktopOverlaySceneConfig.MinimumHeight, Math.Min(height, bounds.Height));
        var maxLeft = bounds.Right - clampedWidth;
        var maxTop = bounds.Bottom - clampedHeight;

        return new GameViewport(
            Math.Min(Math.Max(left, bounds.Left), maxLeft),
            Math.Min(Math.Max(top, bounds.Top), maxTop),
            clampedWidth,
            clampedHeight);
    }

    public static GameViewport CenterInBounds(Rectangle bounds)
    {
        var width = Math.Max(DesktopOverlaySceneConfig.MinimumWidth, Math.Min(DesktopOverlaySceneConfig.DefaultWidth, bounds.Width));
        var height = Math.Max(DesktopOverlaySceneConfig.MinimumHeight, Math.Min(DesktopOverlaySceneConfig.DefaultHeight, bounds.Height));
        var left = bounds.Left + (bounds.Width - width) / 2.0;
        var top = bounds.Top + (bounds.Height - height) / 2.0;

        return new GameViewport(left, top, width, height);
    }
}
