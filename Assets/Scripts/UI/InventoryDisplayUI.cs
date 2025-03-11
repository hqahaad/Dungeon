using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private RectTransform viewRect;
    [SerializeField] private RectTransform contentRect;

    private List<InventorySlotUI> slots = new();

    void Awake()
    {
        Close();

        //Sample
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();

        inventory.OnChangedItemSlot += ChangedSlot;
    }

    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            slots.Add(Instantiate(slotPrefab, contentRect));
        }
    }

    private void ChangedSlot(int index, ItemSlot slot)
    {
        Debug.Log($"{index}°¡ {slot.Count}°³");
        slots[index].OnChangedSlot(slot);
    }


    public void Open() => viewRect.gameObject.SetActive(true);

    public void Close() => viewRect.gameObject.SetActive(false);

    public void Toggle()
    {
        Cursor.visible = !viewRect.gameObject.activeInHierarchy;
        viewRect.gameObject.SetActive(!viewRect.gameObject.activeInHierarchy);
    }
}