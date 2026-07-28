using System;
using System.Collections.Generic;

namespace TaskbarHeroOverlay.Game.Entities.Characters.Skills;

public sealed class CharacterSkillDefinition
{
    public CharacterSkillDefinition(
        string id,
        string displayName,
        string description,
        CharacterSkillDeliveryType deliveryType,
        CharacterSkillTargetingType targetingType,
        CharacterSkillCooldownDefinition cooldown,
        CharacterSkillCostDefinition cost,
        CharacterSkillCastDefinition cast,
        IReadOnlyCollection<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Skill id is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Skill display name is required.", nameof(displayName));
        }

        Id = id;
        DisplayName = displayName;
        Description = description;
        DeliveryType = deliveryType;
        TargetingType = targetingType;
        Cooldown = cooldown;
        Cost = cost;
        Cast = cast;
        Tags = tags ?? Array.Empty<string>();
    }

    public string Id { get; }

    public string DisplayName { get; }

    public string Description { get; }

    public CharacterSkillDeliveryType DeliveryType { get; }

    public CharacterSkillTargetingType TargetingType { get; }

    public CharacterSkillCooldownDefinition Cooldown { get; }

    public CharacterSkillCostDefinition Cost { get; }

    public CharacterSkillCastDefinition Cast { get; }

    public IReadOnlyCollection<string> Tags { get; }
}
