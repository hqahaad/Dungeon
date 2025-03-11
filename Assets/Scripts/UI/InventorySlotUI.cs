using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI countTmp;

    public void SetItemSprite(Sprite sprite) => itemImage.sprite = sprite;
    public void SetCount(int count) => countTmp.text = count.ToString();

    void Start()
    {
        countTmp.gameObject.SetActive(false);    
    }

    public void OnChangedSlot(ItemSlot slot)
    {
        SetItemSprite(slot.ItemInstance.ItemData.ItemIcon);
        SetCount(slot.Count);

        var c = itemImage.color;
        itemImage.color = new Color(c.r, c.g, c.b, 1f);
        countTmp.gameObject.SetActive(slot.Count > 1 ? true : false);

        if (slot.ItemInstance is NullItem)
        {
            //Exception
        }
    }
}
