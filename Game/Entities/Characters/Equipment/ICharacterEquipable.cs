using System.Collections.Generic;
using TaskbarHeroOverlay.Game.Entities.Characters.Stats;

namespace TaskbarHeroOverlay.Game.Entities.Characters.Equipment;

public interface ICharacterEquipable
{
    string Id { get; }

    string DisplayName { get; }

    IReadOnlyCollection<CharacterEquipmentSlotId> AllowedSlots { get; }

    CharacterStatBlock GrantedStats { get; }
}
