using TaskbarHeroOverlay.Game.Entities.Characters.Hero;

namespace TaskbarHeroOverlay.Game.Scenes.DesktopOverlay;

public sealed class DesktopOverlaySceneState
{
    public HeroState Hero { get; } = new()
    {
        X = HeroMotionConfig.StartX,
    };
}
