using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Threading;
using TaskbarHeroOverlay.Game.Core;
using TaskbarHeroOverlay.Game.Rendering;
using TaskbarHeroOverlay.Game.Scenes.DesktopOverlay;
using TaskbarHeroOverlay.Game.Systems.Characters;
using TaskbarHeroOverlay.Game.Systems.Layout;
using TaskbarHeroOverlay.Config;
using TaskbarHeroOverlay.UI.Editing;
using TaskbarHeroOverlay.UI.Rendering;

namespace TaskbarHeroOverlay.UI.Windows;

public partial class GameWindow : Window
{
    private readonly GameWindowEditor _editor;
    private readonly DesktopOverlaySceneRenderer _sceneRenderer;
    private readonly DispatcherTimer _timer;
    private readonly DesktopOverlaySceneState _sceneState = new();

    public bool IsEditMode { get; private set; }
    public event EventHandler<bool>? EditModeChanged;

    public GameWindow()
    {
        InitializeComponent();
        _editor = new GameWindowEditor(this, EditorChrome, ApplyLayout);
        _sceneRenderer = new DesktopOverlaySceneRenderer(OverlayCanvas, Hero);
        ApplyWindowConfiguration();

        SourceInitialized += OnSourceInitialized;
        Loaded += OnLoaded;
        SizeChanged += (_, _) => UpdateOverlaySize();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(FrameTiming.FrameIntervalMilliseconds),
        };
        _timer.Tick += OnTick;
        _timer.Start();
    }

    private void ApplyWindowConfiguration()
    {
        Title = AppConfig.ProductName;
        MinWidth = DesktopOverlaySceneConfig.MinimumWidth;
        MinHeight = DesktopOverlaySceneConfig.MinimumHeight;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        CenterOnPrimaryScreen();
        RenderCurrentScene();
    }

    public void ApplyLayout(double left, double top, double width, double height)
    {
        var viewport = GameViewportLayout.ClampToBounds(left, top, width, height, ScreenBoundsProvider.GetPrimaryScreenBounds());
        Left = viewport.Left;
        Top = viewport.Top;
        Width = viewport.Width;
        Height = viewport.Height;
        UpdateOverlaySize();
    }

    public void SetEditMode(bool isEditMode)
    {
        if (IsEditMode == isEditMode)
        {
            return;
        }

        IsEditMode = isEditMode;
        if (_editor.SetEditMode(isEditMode))
        {
            EditModeChanged?.Invoke(this, IsEditMode);
        }
    }

    public void CenterOnPrimaryScreen()
    {
        var viewport = GameViewportLayout.CenterInBounds(ScreenBoundsProvider.GetPrimaryScreenBounds());
        ApplyLayout(viewport.Left, viewport.Top, viewport.Width, viewport.Height);
    }

    private void UpdateOverlaySize()
    {
        _sceneRenderer.ResizeViewport(Width, Height);
        RenderCurrentScene();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (_sceneRenderer.SceneWidth <= 0)
        {
            return;
        }

        HeroMotionSystem.Update(_sceneState.Hero, _timer.Interval.TotalSeconds, _sceneRenderer.SceneWidth, _sceneRenderer.HeroWidth);
        RenderCurrentScene();
    }

    private void RenderCurrentScene()
    {
        _sceneRenderer.Render(_sceneState);
    }

    private void OnSourceInitialized(object? sender, EventArgs e)
    {
        _editor.InitializeHandle(new WindowInteropHelper(this).Handle);
    }

    private void MoveThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.Move(e.HorizontalChange, e.VerticalChange);
    }

    private void TopLeftThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeLeft(e.HorizontalChange, DesktopOverlaySceneConfig.MinimumWidth);
        _editor.ResizeTop(e.VerticalChange, DesktopOverlaySceneConfig.MinimumHeight);
    }

    private void TopThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeTop(e.VerticalChange, DesktopOverlaySceneConfig.MinimumHeight);
    }

    private void TopRightThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeRight(e.HorizontalChange, DesktopOverlaySceneConfig.MinimumWidth);
        _editor.ResizeTop(e.VerticalChange, DesktopOverlaySceneConfig.MinimumHeight);
    }

    private void LeftThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeLeft(e.HorizontalChange, DesktopOverlaySceneConfig.MinimumWidth);
    }

    private void RightThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeRight(e.HorizontalChange, DesktopOverlaySceneConfig.MinimumWidth);
    }

    private void BottomLeftThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeLeft(e.HorizontalChange, DesktopOverlaySceneConfig.MinimumWidth);
        _editor.ResizeBottom(e.VerticalChange, DesktopOverlaySceneConfig.MinimumHeight);
    }

    private void BottomThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeBottom(e.VerticalChange, DesktopOverlaySceneConfig.MinimumHeight);
    }

    private void BottomRightThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        _editor.ResizeRight(e.HorizontalChange, DesktopOverlaySceneConfig.MinimumWidth);
        _editor.ResizeBottom(e.VerticalChange, DesktopOverlaySceneConfig.MinimumHeight);
    }
}
