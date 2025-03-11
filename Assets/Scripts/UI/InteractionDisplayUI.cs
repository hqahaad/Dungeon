using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTmp;
    [SerializeField] private TextMeshProUGUI descTmp;
    [SerializeField] private GameObject displayBorder;

    void OnEnable()
    {
        displayBorder.gameObject.SetActive(false);
    }

    public void SetDisplay(IInteractable interactable)
    {
        if (interactable == null)
        {
            displayBorder.gameObject.SetActive(false);
            return;
        }

        displayBorder.gameObject.SetActive(true);

        nameTmp.text = interactable.GetName();
        descTmp.text = interactable.GetDescription();
    }
}
