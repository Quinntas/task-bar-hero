using System.Windows;
using TaskbarHeroOverlay.Config;

namespace TaskbarHeroOverlay.UI.Windows;

public partial class ControlWindow : Window
{
    private readonly GameWindow _gameWindow;

    public ControlWindow(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
        InitializeComponent();
        ApplyWindowConfiguration();
        Loaded += (_, _) => RefreshEditModeButton();
        _gameWindow.EditModeChanged += OnGameWindowEditModeChanged;
    }

    private void ApplyWindowConfiguration()
    {
        Title = AppConfig.ControlWindowTitle;
        Width = WindowConfig.ControlWindowWidth;
        Height = WindowConfig.ControlWindowHeight;
        MinWidth = WindowConfig.ControlWindowMinimumWidth;
        MinHeight = WindowConfig.ControlWindowMinimumHeight;
    }

    protected override void OnClosed(System.EventArgs e)
    {
        _gameWindow.EditModeChanged -= OnGameWindowEditModeChanged;
        base.OnClosed(e);
    }

    private void EditModeClicked(object sender, RoutedEventArgs e)
    {
        _gameWindow.SetEditMode(!_gameWindow.IsEditMode);
        RefreshEditModeButton();
    }

    private void OnGameWindowEditModeChanged(object? sender, bool isEditMode)
    {
        RefreshEditModeButton();
    }

    private void RefreshEditModeButton()
    {
        EditModeButton.Content = _gameWindow.IsEditMode
            ? AppConfig.FinishEditModeButtonText
            : AppConfig.EditModeButtonText;
    }
}
