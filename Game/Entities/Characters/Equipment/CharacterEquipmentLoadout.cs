using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskbarHeroOverlay.Game.Entities.Characters.Equipment;

public sealed class CharacterEquipmentLoadout
{
    private readonly Dictionary<CharacterEquipmentSlotId, ICharacterEquipable?> _itemsBySlot;

    public CharacterEquipmentLoadout(IReadOnlyList<CharacterEquipmentSlotDefinition> slotDefinitions)
    {
        SlotDefinitions = slotDefinitions;
        _itemsBySlot = slotDefinitions.ToDictionary(slot => slot.SlotId, _ => default(ICharacterEquipable));
    }

    public IReadOnlyList<CharacterEquipmentSlotDefinition> SlotDefinitions { get; }

    public IReadOnlyDictionary<CharacterEquipmentSlotId, ICharacterEquipable?> ItemsBySlot =>
        new ReadOnlyDictionary<CharacterEquipmentSlotId, ICharacterEquipable?>(_itemsBySlot);

    public ICharacterEquipable? Get(CharacterEquipmentSlotId slotId)
    {
        return _itemsBySlot.GetValueOrDefault(slotId);
    }

    public void Equip(CharacterEquipmentSlotId slotId, ICharacterEquipable item)
    {
        if (!_itemsBySlot.ContainsKey(slotId))
        {
            throw new InvalidOperationException($"Slot '{slotId}' is not available for this character.");
        }

        if (!item.AllowedSlots.Contains(slotId))
        {
            throw new InvalidOperationException($"Item '{item.Id}' cannot be equipped in slot '{slotId}'.");
        }

        _itemsBySlot[slotId] = item;
    }

    public ICharacterEquipable? Unequip(CharacterEquipmentSlotId slotId)
    {
        if (!_itemsBySlot.TryGetValue(slotId, out var existingItem))
        {
            throw new InvalidOperationException($"Slot '{slotId}' is not available for this character.");
        }

        _itemsBySlot[slotId] = null;
        return existingItem;
    }
}
