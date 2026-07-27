using System;
using TaskbarHeroOverlay.Game.Entities.Characters.Hero;

namespace TaskbarHeroOverlay.Game.Systems.Characters;

public static class HeroMotionSystem
{
    public static void Update(HeroState hero, double deltaSeconds, double sceneWidth, double heroWidth)
    {
        if (sceneWidth <= 0)
        {
            return;
        }

        hero.X += HeroMotionConfig.Speed * hero.Direction * deltaSeconds;

        var maxLeft = Math.Max(0, sceneWidth - heroWidth);

        if (hero.X >= maxLeft)
        {
            hero.X = maxLeft;
            hero.Direction = -1;
        }
        else if (hero.X <= 0)
        {
            hero.X = 0;
            hero.Direction = 1;
        }
    }
}
