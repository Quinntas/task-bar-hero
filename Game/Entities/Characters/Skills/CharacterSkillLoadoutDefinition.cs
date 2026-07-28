using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskbarHeroOverlay.Game.Entities.Characters.Skills;

public sealed class CharacterSkillLoadoutDefinition
{
    private readonly IReadOnlyDictionary<CharacterSkillSlotId, CharacterSkillDefinition> _skillsBySlot;

    public CharacterSkillLoadoutDefinition(
        CharacterSkillDefinition skill1,
        CharacterSkillDefinition skill2,
        CharacterSkillDefinition skill3,
        CharacterSkillDefinition skill4)
    {
        _skillsBySlot = new ReadOnlyDictionary<CharacterSkillSlotId, CharacterSkillDefinition>(
            new Dictionary<CharacterSkillSlotId, CharacterSkillDefinition>
            {
                [CharacterSkillSlotId.Skill1] = skill1,
                [CharacterSkillSlotId.Skill2] = skill2,
                [CharacterSkillSlotId.Skill3] = skill3,
                [CharacterSkillSlotId.Skill4] = skill4,
            });

        var duplicateIds = _skillsBySlot.Values
            .GroupBy(skill => skill.Id)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateIds.Count > 0)
        {
            throw new ArgumentException($"Character skills must be unique. Duplicate ids: {string.Join(", ", duplicateIds)}");
        }
    }

    public IReadOnlyDictionary<CharacterSkillSlotId, CharacterSkillDefinition> SkillsBySlot => _skillsBySlot;

    public CharacterSkillDefinition Get(CharacterSkillSlotId slotId)
    {
        return _skillsBySlot[slotId];
    }
}
