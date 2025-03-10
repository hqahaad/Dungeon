using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public StatMediator<StatType> Mediator { get; private set; } = new();

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float maxHp;

    [Header("SO Events")]
    [SerializeField] private FloatGameEvent healthEvent;

    protected float currentHp;

    public float MoveSpeed
    {
        get
        {
            var query = new ModifierQuery<StatType>(StatType.MoveSpeed, moveSpeed);
            Mediator.PerformQuery(this, query);

            return query.Value;
        }
    }

    public float JumpPower
    {
        get
        {
            var query = new ModifierQuery<StatType>(StatType.JumpPower, jumpPower);
            Mediator.PerformQuery(this, query);

            return query.Value;
        }
    }

    public float Hp
    {
        get
        {
            return Mathf.Clamp(currentHp, 0f, maxHp);
        }
        set
        {
            if (value == currentHp)
                return;

            currentHp = Mathf.Clamp(value, 0f, maxHp);
            healthEvent?.Raise(Hp / maxHp);
        }
    }

    void Start()
    {
        SetUpStat();
    }

    public void Update() => Mediator.Update(Time.deltaTime);

    private void SetUpStat()
    {
        Hp = maxHp;
    }
}

public enum StatType
{
    MoveSpeed,
    JumpPower,
    MaxHp
}