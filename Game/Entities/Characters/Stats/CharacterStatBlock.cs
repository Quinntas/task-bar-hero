using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskbarHeroOverlay.Game.Entities.Characters.Stats;

public sealed class CharacterStatBlock
{
    private readonly Dictionary<CharacterStatType, double> _values;

    public CharacterStatBlock()
    {
        _values = Enum.GetValues<CharacterStatType>().ToDictionary(stat => stat, _ => 0d);
    }

    public IReadOnlyDictionary<CharacterStatType, double> Values =>
        new ReadOnlyDictionary<CharacterStatType, double>(_values);

    public double Get(CharacterStatType statType)
    {
        return _values.GetValueOrDefault(statType);
    }

    public void Set(CharacterStatType statType, double value)
    {
        _values[statType] = value;
    }

    public void Add(CharacterStatType statType, double value)
    {
        _values[statType] = Get(statType) + value;
    }

    public CharacterStatBlock Clone()
    {
        var clone = new CharacterStatBlock();

        foreach (var entry in _values)
        {
            clone.Set(entry.Key, entry.Value);
        }

        return clone;
    }

    public static CharacterStatBlock Combine(params CharacterStatBlock[] statBlocks)
    {
        var combined = new CharacterStatBlock();

        foreach (var statBlock in statBlocks)
        {
            foreach (var entry in statBlock._values)
            {
                combined.Add(entry.Key, entry.Value);
            }
        }

        return combined;
    }
}
