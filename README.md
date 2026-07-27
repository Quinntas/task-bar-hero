# Taskbar Hero Overlay

Windows desktop overlay prototype built with `C#`, `WPF`, and `Win32 interop`.

The app currently runs as two windows:
- a transparent game window that renders the overlay character
- a normal control window used to toggle screen edit mode

## Current Features

- transparent overlay game window
- always-on-top desktop overlay behavior
- hidden from the taskbar
- click-through when not editing
- edit mode with:
  - red border highlight
  - resize handles on corners and edges
  - drag-to-move behavior
- game window clamped to the primary monitor
- minimum game window size of `640x480`
- system tray icon with `Quit`
- separate game and control windows

## Tech Stack

- `.NET 10`
- `WPF`
- `System.Windows.Forms.NotifyIcon` for tray support
- `Win32` interop for layered/tool/click-through window behavior

## Run

```bash
dotnet run
```

## Build

```bash
dotnet build
```

## Project Structure

```text
TaskbarHeroOverlay
├── Assets/
│   ├── Characters/
│   ├── Items/
│   ├── Maps/
│   └── UI/
├── Config/
├── Game/
│   ├── Core/
│   ├── Entities/
│   ├── Rendering/
│   ├── Scenes/
│   └── Systems/
├── Interop/
├── UI/
│   └── Windows/
├── App.xaml
├── App.xaml.cs
└── TaskbarHeroOverlay.csproj
```

## Folder Responsibilities

### `Config/`

Application and window configuration values.

- `AppConfig.cs`: app titles and control button labels
- `TrayConfig.cs`: tray text and menu labels
- `WindowConfig.cs`: control-window sizing defaults

### `Interop/`

Native Windows interop used by the overlay window.

- `NativeMethods.cs`: `GetWindowLongPtr`, `SetWindowLongPtr`, `SetWindowPos`
- `NativeWindowConfig.cs`: Win32 style and flag constants

### `UI/Windows/`

WPF shell windows only.

- `GameWindow.xaml(.cs)`: overlay host window and edit chrome
- `ControlWindow.xaml(.cs)`: normal control popup with edit toggle

### `Game/`

Game-facing logic separated from WPF host code.

- `Core/`: shared primitives and environment helpers
- `Entities/`: runtime entity state such as hero state
- `Scenes/`: scene config and scene state
- `Systems/`: frame update logic and layout systems
- `Rendering/`: frame timing and renderer-facing constants

### `Assets/`

Game asset layout placeholders for future content.

- `Characters/<CharacterName>/`
- `Items/<ItemName>/`
- `Maps/<MapName>/`
- `UI/<AssetGroup>/`

Each asset should eventually own its own metadata, sprite sheet, animation data, and related files.
