using System.Collections.Generic;
using System.Linq;
using TaskbarHeroOverlay.Game.Entities.Characters.Equipment;
using TaskbarHeroOverlay.Game.Entities.Characters.Stats;

namespace TaskbarHeroOverlay.Game.Entities.Characters;

public sealed class CharacterState
{
    public CharacterState(CharacterDefinition definition)
    {
        Definition = definition;
        Equipment = new CharacterEquipmentLoadout(definition.EquipmentSlots);
        BonusStats = new CharacterStatBlock();
    }

    public CharacterDefinition Definition { get; }

    public int Level { get; set; } = 1;

    public CharacterEquipmentLoadout Equipment { get; }

    public CharacterStatBlock BonusStats { get; }

    public CharacterStatBlock GetTotalStats()
    {
        var equipmentStatBlocks = Equipment.ItemsBySlot.Values
            .Where(item => item is not null)
            .Select(item => item!.GrantedStats)
            .ToList();

        var statBlocks = new List<CharacterStatBlock>(equipmentStatBlocks.Count + 2)
        {
            Definition.BaseStats,
            BonusStats,
        };

        statBlocks.AddRange(equipmentStatBlocks);
        return CharacterStatBlock.Combine([.. statBlocks]);
    }
}
