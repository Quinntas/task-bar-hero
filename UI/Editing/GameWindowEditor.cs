using System;
using System.Windows;
using TaskbarHeroOverlay.Interop;

namespace TaskbarHeroOverlay.UI.Editing;

public sealed class GameWindowEditor
{
    private readonly Window _window;
    private readonly FrameworkElement _editorChrome;
    private readonly Action<double, double, double, double> _applyLayout;
    private IntPtr _handle;

    public bool IsEditMode { get; private set; }

    public GameWindowEditor(Window window, FrameworkElement editorChrome, Action<double, double, double, double> applyLayout)
    {
        _window = window;
        _editorChrome = editorChrome;
        _applyLayout = applyLayout;
    }

    public void InitializeHandle(IntPtr handle)
    {
        _handle = handle;
        UpdateWindowInteractivity();
    }

    public bool SetEditMode(bool isEditMode)
    {
        if (IsEditMode == isEditMode)
        {
            return false;
        }

        IsEditMode = isEditMode;
        _editorChrome.Visibility = isEditMode ? Visibility.Visible : Visibility.Collapsed;
        _editorChrome.IsHitTestVisible = isEditMode;
        UpdateWindowInteractivity();
        return true;
    }

    public void Move(double horizontalChange, double verticalChange)
    {
        _applyLayout(_window.Left + horizontalChange, _window.Top + verticalChange, _window.Width, _window.Height);
    }

    public void ResizeLeft(double delta, double minimumWidth)
    {
        var newWidth = Math.Max(minimumWidth, _window.Width - delta);
        var newLeft = _window.Left + (_window.Width - newWidth);
        _applyLayout(newLeft, _window.Top, newWidth, _window.Height);
    }

    public void ResizeRight(double delta, double minimumWidth)
    {
        _applyLayout(_window.Left, _window.Top, Math.Max(minimumWidth, _window.Width + delta), _window.Height);
    }

    public void ResizeTop(double delta, double minimumHeight)
    {
        var newHeight = Math.Max(minimumHeight, _window.Height - delta);
        var newTop = _window.Top + (_window.Height - newHeight);
        _applyLayout(_window.Left, newTop, _window.Width, newHeight);
    }

    public void ResizeBottom(double delta, double minimumHeight)
    {
        _applyLayout(_window.Left, _window.Top, _window.Width, Math.Max(minimumHeight, _window.Height + delta));
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
}
