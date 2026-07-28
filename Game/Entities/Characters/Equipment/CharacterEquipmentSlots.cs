using System;
using System.Collections.Generic;

namespace TaskbarHeroOverlay.Game.Entities.Characters.Equipment;

public static class CharacterEquipmentSlots
{
    public static IReadOnlyList<CharacterEquipmentSlotDefinition> DefaultCharacterSlots { get; } =
    [
        new(CharacterEquipmentSlotId.HeadArmor, "Head Armor"),
        new(CharacterEquipmentSlotId.TrinketLeft, "Trinket 1"),
        new(CharacterEquipmentSlotId.TrinketRight, "Trinket 2"),
        new(CharacterEquipmentSlotId.EarringLeft, "Earring 1"),
        new(CharacterEquipmentSlotId.EarringRight, "Earring 2"),
        new(CharacterEquipmentSlotId.BodyArmor, "Body Armor"),
        new(CharacterEquipmentSlotId.BracerArmor, "Bracer Armor"),
        new(CharacterEquipmentSlotId.LegArmor, "Leg Armor"),
        new(CharacterEquipmentSlotId.FootArmor, "Foot Armor"),
        new(CharacterEquipmentSlotId.MainHand, "Main Hand"),
        new(CharacterEquipmentSlotId.OffHand, "Off Hand"),
        new(CharacterEquipmentSlotId.Necklace, "Necklace"),
    ];

    public static bool IsDefined(CharacterEquipmentSlotId slotId)
    {
        return Enum.IsDefined(slotId);
    }
}
