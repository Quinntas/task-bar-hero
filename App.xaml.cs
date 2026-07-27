using System.Drawing;
using System.Windows;
using TaskbarHeroOverlay.Config;
using TaskbarHeroOverlay.UI.Windows;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenuStrip = System.Windows.Forms.ContextMenuStrip;

namespace TaskbarHeroOverlay;

public partial class App : System.Windows.Application
{
    private GameWindow? _gameWindow;
    private NotifyIcon? _notifyIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _gameWindow = new GameWindow();
        _gameWindow.Show();

        var controlWindow = new ControlWindow(_gameWindow);
        controlWindow.Closed += (_, _) => Shutdown();
        MainWindow = controlWindow;
        controlWindow.Show();

        _notifyIcon = new NotifyIcon
        {
            Text = TrayConfig.TrayText,
            Icon = SystemIcons.Application,
            Visible = true,
            ContextMenuStrip = BuildTrayMenu(),
        };
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_notifyIcon is not null)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }

        base.OnExit(e);
    }

    private ContextMenuStrip BuildTrayMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add(TrayConfig.QuitMenuText, null, (_, _) => Shutdown());
        return menu;
    }
}
