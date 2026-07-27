using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using TaskbarHeroOverlay.Game.Core;
using TaskbarHeroOverlay.Game.Entities.Characters.Hero;
using TaskbarHeroOverlay.Game.Rendering;
using TaskbarHeroOverlay.Game.Scenes.DesktopOverlay;
using TaskbarHeroOverlay.Game.Systems.Characters;
using TaskbarHeroOverlay.Game.Systems.Layout;
using TaskbarHeroOverlay.Interop;
using TaskbarHeroOverlay.Config;

namespace TaskbarHeroOverlay.UI.Windows;

public partial class GameWindow : Window
{
    private readonly DispatcherTimer _timer;
    private readonly DesktopOverlaySceneState _sceneState = new();
    private IntPtr _handle;

    public bool IsEditMode { get; private set; }
    public event EventHandler<bool>? EditModeChanged;

    public GameWindow()
    {
        InitializeComponent();
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
        RenderScene();
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
        EditorChrome.Visibility = isEditMode ? Visibility.Visible : Visibility.Collapsed;
        EditorChrome.IsHitTestVisible = isEditMode;
        UpdateWindowInteractivity();
        EditModeChanged?.Invoke(this, IsEditMode);
    }

    public void CenterOnPrimaryScreen()
    {
        var viewport = GameViewportLayout.CenterInBounds(ScreenBoundsProvider.GetPrimaryScreenBounds());
        ApplyLayout(viewport.Left, viewport.Top, viewport.Width, viewport.Height);
    }

    private void UpdateOverlaySize()
    {
        OverlayCanvas.Width = Width;
        OverlayCanvas.Height = Height;
        RenderScene();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (OverlayCanvas.ActualWidth <= 0)
        {
            return;
        }

        HeroMotionSystem.Update(_sceneState.Hero, _timer.Interval.TotalSeconds, OverlayCanvas.ActualWidth, Hero.ActualWidth);
        RenderScene();
    }

    private void RenderScene()
    {
        if (OverlayCanvas.ActualHeight <= 0)
        {
            return;
        }

        Canvas.SetLeft(Hero, _sceneState.Hero.X);

        var top = Math.Max(0, OverlayCanvas.ActualHeight - Hero.ActualHeight - HeroMotionConfig.GroundMargin);
        Canvas.SetTop(Hero, top);
        Hero.RenderTransform = new ScaleTransform(_sceneState.Hero.Direction, 1, Hero.ActualWidth / 2, Hero.ActualHeight / 2);
    }

    private void OnSourceInitialized(object? sender, EventArgs e)
    {
        _handle = new WindowInteropHelper(this).Handle;
        UpdateWindowInteractivity();
    }

    private void UpdateWindowInteractivity()
    {
        if (_handle == IntPtr.Zero)
        {
            return;
        }

        var extendedStyle = NativeMethods.GetWindowLongPtr(_handle, NativeWindowConfig.GwlExstyle).ToInt64();
        extendedStyle |= NativeWindowConfig.WsExLayered | NativeWindowConfig.WsExToolwindow;

        if (IsEditMode)
        {
            extendedStyle &= ~NativeWindowConfig.WsExTransparent;
        }
        else
        {
            extendedStyle |= NativeWindowConfig.WsExTransparent;
        }

        NativeMethods.SetWindowLongPtr(_handle, NativeWindowConfig.GwlExstyle, new IntPtr(extendedStyle));
        NativeMethods.SetWindowPos(
            _handle,
            IntPtr.Zero,
            0,
            0,
            0,
            0,
            NativeWindowConfig.SwpNomove
            | NativeWindowConfig.SwpNosize
            | NativeWindowConfig.SwpNozorder
            | NativeWindowConfig.SwpNoactivate
            | NativeWindowConfig.SwpFramechanged);
    }

    private void MoveThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ApplyLayout(Left + e.HorizontalChange, Top + e.VerticalChange, Width, Height);
    }

    private void TopLeftThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeLeft(e.HorizontalChange);
        ResizeTop(e.VerticalChange);
    }

    private void TopThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeTop(e.VerticalChange);
    }

    private void TopRightThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeRight(e.HorizontalChange);
        ResizeTop(e.VerticalChange);
    }

    private void LeftThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeLeft(e.HorizontalChange);
    }

    private void RightThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeRight(e.HorizontalChange);
    }

    private void BottomLeftThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeLeft(e.HorizontalChange);
        ResizeBottom(e.VerticalChange);
    }

    private void BottomThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeBottom(e.VerticalChange);
    }

    private void BottomRightThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        ResizeRight(e.HorizontalChange);
        ResizeBottom(e.VerticalChange);
    }

    private void ResizeLeft(double delta)
    {
        var newWidth = Math.Max(DesktopOverlaySceneConfig.MinimumWidth, Width - delta);
        var newLeft = Left + (Width - newWidth);
        ApplyLayout(newLeft, Top, newWidth, Height);
    }

    private void ResizeRight(double delta)
    {
        ApplyLayout(Left, Top, Math.Max(DesktopOverlaySceneConfig.MinimumWidth, Width + delta), Height);
    }

    private void ResizeTop(double delta)
    {
        var newHeight = Math.Max(DesktopOverlaySceneConfig.MinimumHeight, Height - delta);
        var newTop = Top + (Height - newHeight);
        ApplyLayout(Left, newTop, Width, newHeight);
    }

    private void ResizeBottom(double delta)
    {
        ApplyLayout(Left, Top, Width, Math.Max(DesktopOverlaySceneConfig.MinimumHeight, Height + delta));
    }
}
