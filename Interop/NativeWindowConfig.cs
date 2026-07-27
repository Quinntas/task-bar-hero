namespace TaskbarHeroOverlay.Interop;

internal static class NativeWindowConfig
{
    public const int GwlExstyle = -20;
    public const int WsExTransparent = 0x20;
    public const int WsExToolwindow = 0x80;
    public const int WsExLayered = 0x80000;
    public const uint SwpNomove = 0x0002;
    public const uint SwpNosize = 0x0001;
    public const uint SwpNozorder = 0x0004;
    public const uint SwpNoactivate = 0x0010;
    public const uint SwpFramechanged = 0x0020;
}
