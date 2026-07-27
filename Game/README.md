# Game Layer

- `Core/`: shared game primitives and environment helpers
- `Entities/`: characters, items, and world objects
- `Scenes/`: scene-specific rules and defaults
- `Systems/`: gameplay and layout systems
- `Rendering/`: frame timing and renderer-facing state

This layer should stay independent from WPF window concerns where possible.

Current split:
- scene state lives in `Scenes/`
- entity state lives in `Entities/`
- per-frame behavior lives in `Systems/`
