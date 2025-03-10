using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatModifier<T> : IDisposable where T : Enum 
{
    public T StatType { get; }
    public IOperationStrategy Strategy { get; }
    public bool MarkedForRemoval { get; set; }

    public event Action<StatModifier<T>> OnDispose = delegate { };

    private bool useTimer = false;
    private float turnTimer;

    public StatModifier(T statType, IOperationStrategy strategy, float duration = 0f)
    {
        StatType = statType;
        Strategy = strategy;

        useTimer = duration == 0f ? false : true;
        turnTimer = duration;
    }

    public void Handle(object sender, ModifierQuery<T> query)
    {
        if (StatType.Equals(query.StatType))
        {
            query.Value = Strategy.Culculate(query.Value);
        }
    }

    public void Update(float deltaTime)
    {
        if (!useTimer)
            return;

        turnTimer -= deltaTime;

        if (turnTimer < 0f)
        {
            MarkedForRemoval = true;
        }
    }

    public void Dispose()
    {
        OnDispose?.Invoke(this);
    }
}

public class ModifierQuery<T> where T : Enum
{
    public T StatType { get; }
    public float Value { get; set; }

    public ModifierQuery(T statType, float value)
    {
        StatType = statType;
        Value = value;
    }
}