using UnityEngine;

public class ModifierStat : MonoBehaviour, IInteractable
{
    [SerializeField] private StatType statType;
    [SerializeField] private StatOperationType operationType;
    [SerializeField] private float value;
    [SerializeField] private float duration;
    [SerializeField] private bool destroyFromInteraction = true;

    public void Interaction(PlayerController player)
    {
        if (operationType == StatOperationType.Add)
        {
            player.Stat.Mediator.AddModifier(
                new StatModifier<StatType>(statType, new AddOperation(value), duration));
        }
        else
        {
            player.Stat.Mediator.AddModifier(
                new StatModifier<StatType>(statType, new MultiplyOperation(value), duration));
        }

        if (destroyFromInteraction)
        {
            Destroy(this.gameObject);
        }
    }

    public string GetName() => $"{statType.ToString()} booster";
    public string GetDescription() => $"boosting {statType.ToString()} ( {operationType.ToString()} )";

    public enum StatOperationType
    {
        Add,
        Multiply
    }
}
