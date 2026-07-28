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

Character domain foundations:
- `Entities/Characters/CharacterDefinition.cs`: immutable character definition
- `Entities/Characters/CharacterState.cs`: mutable runtime character state
- `Entities/Characters/Stats/`: stat keys and stat blocks
- `Entities/Characters/Equipment/`: slot definitions, loadout, and equipable contract
- `Entities/Characters/Skills/`: unique per-character 4-skill definitions

Character rules currently modeled:
- each character owns exactly 4 unique skills
- equipment acts as stat-stick gear only via granted stats

Skill metadata currently modeled:
- delivery type
- targeting type
- cooldown definition
- resource cost definition
- cast definition
- arbitrary string tags for later combat filtering
