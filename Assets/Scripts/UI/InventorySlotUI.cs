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
}
