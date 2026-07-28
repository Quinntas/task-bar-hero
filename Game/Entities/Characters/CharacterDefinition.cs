using System;
using System.Collections.Generic;
using TaskbarHeroOverlay.Game.Entities.Characters.Equipment;
using TaskbarHeroOverlay.Game.Entities.Characters.Skills;
using TaskbarHeroOverlay.Game.Entities.Characters.Stats;

namespace TaskbarHeroOverlay.Game.Entities.Characters;

public sealed class CharacterDefinition
{
    public CharacterDefinition(
        string id,
        string displayName,
        CharacterStatBlock baseStats,
        CharacterSkillLoadoutDefinition skills,
        IReadOnlyList<CharacterEquipmentSlotDefinition>? equipmentSlots = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Character id is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Character display name is required.", nameof(displayName));
        }

        Id = id;
        DisplayName = displayName;
        BaseStats = baseStats;
        Skills = skills;
        EquipmentSlots = equipmentSlots ?? CharacterEquipmentSlots.DefaultCharacterSlots;
    }

    public string Id { get; }

    public string DisplayName { get; }

    public CharacterStatBlock BaseStats { get; }

    public CharacterSkillLoadoutDefinition Skills { get; }

    public IReadOnlyList<CharacterEquipmentSlotDefinition> EquipmentSlots { get; }
}
