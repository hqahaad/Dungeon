using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public readonly StatMediator<StatType> mediator = new();

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float maxHp;

    public float MoveSpeed
    {
        get
        {
            var query = new ModifierQuery<StatType>(StatType.MoveSpeed, moveSpeed);
            mediator.PerformQuery(this, query);

            return query.Value;
        }
    }

    public float JumpPower
    {
        get
        {
            var query = new ModifierQuery<StatType>(StatType.JumpPower, jumpPower);
            mediator.PerformQuery(this, query);

            return query.Value;
        }
    }

    public void Update()
    {
        mediator.Update(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.V))
        {
            mediator.AddModifier(new StatModifier<StatType>(StatType.MoveSpeed, new AddOperation(10), 3f));
        }
    }
}

public enum StatType
{
    MoveSpeed,
    JumpPower,
    MaxHp
}