using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class StatMediator<T> where T : Enum
{
    private readonly List<StatModifier<T>> modifiers = new();
    private Dictionary<T, IEnumerable<StatModifier<T>>> modifiersCache = new();
    private readonly IStatModifierOrder<T> order = new NormalStatModifierOrder<T>();

    public void PerformQuery(object sender, ModifierQuery<T> query)
    {
        if (!modifiersCache.ContainsKey(query.StatType))
        {
            modifiersCache[query.StatType] = modifiers.Where(s => s.StatType.Equals(query.StatType)).ToList();
        }
        query.Value = order.Apply(modifiersCache[query.StatType], query.Value);
    }

    public void AddModifier(StatModifier<T> modifier)
    {
        modifiers.Add(modifier);
        modifiersCache.Remove(modifier.StatType);
        modifier.MarkedForRemoval = false;

        modifier.OnDispose += val => InvalidateCache(modifier.StatType);
        modifier.OnDispose += val => RemoveModifier(modifier);
    }

    public void Update(float deltaTime)
    {
        modifiers.ForEach(s => s.Update(deltaTime));

        foreach (var iter in modifiers.Where(s => s.MarkedForRemoval).ToList())
        {
            iter.Dispose();
        }
    }

    private void InvalidateCache(T statType)
    {
        modifiersCache.Remove(statType);
    }

    private void RemoveModifier(StatModifier<T> modifier)
    {
        modifiers.Remove(modifier);
    }
}