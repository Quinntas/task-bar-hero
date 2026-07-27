using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TaskbarHeroOverlay.Game.Entities.Characters.Hero;
using TaskbarHeroOverlay.Game.Scenes.DesktopOverlay;

namespace TaskbarHeroOverlay.UI.Rendering;

public sealed class DesktopOverlaySceneRenderer
{
    private readonly Canvas _overlayCanvas;
    private readonly FrameworkElement _hero;

    public DesktopOverlaySceneRenderer(Canvas overlayCanvas, FrameworkElement hero)
    {
        _overlayCanvas = overlayCanvas;
        _hero = hero;
    }

    public double SceneWidth => _overlayCanvas.ActualWidth;

    public double HeroWidth => _hero.ActualWidth;

    public void ResizeViewport(double width, double height)
    {
        _overlayCanvas.Width = width;
        _overlayCanvas.Height = height;
    }

    public void Render(DesktopOverlaySceneState sceneState)
    {
        if (_overlayCanvas.ActualHeight <= 0)
        {
            return;
        }

        Canvas.SetLeft(_hero, sceneState.Hero.X);

        var top = Math.Max(0, _overlayCanvas.ActualHeight - _hero.ActualHeight - HeroMotionConfig.GroundMargin);
        Canvas.SetTop(_hero, top);
        _hero.RenderTransform = new ScaleTransform(sceneState.Hero.Direction, 1, _hero.ActualWidth / 2, _hero.ActualHeight / 2);
    }
}
