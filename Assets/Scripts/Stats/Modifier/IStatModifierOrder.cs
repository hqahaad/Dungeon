using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IStatModifierOrder<T> where T : Enum
{
    float Apply(IEnumerable<StatModifier<T>> mods, float baseValue);
}

public class NormalStatModifierOrder<T> : IStatModifierOrder<T> where T : Enum
{
    public float Apply(IEnumerable<StatModifier<T>> mods, float baseValue)
    {
        foreach (var mod in mods.Where(s => s.Strategy is AddOperation))
        {
            baseValue = mod.Strategy.Culculate(baseValue);
        }

        foreach (var mod in mods.Where(s => s.Strategy is MultiplyOperation))
        {
            baseValue = mod.Strategy.Culculate(baseValue);
        }

        return baseValue;
    }
}

