using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public ItemRotator Rotator;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public Button AcceptButton;
    public Button RejectButton;

    private void Start()
    {
        UpdateUI(Rotator.CurrentObject);
        AcceptButton.gameObject.SetActive(false);
        RejectButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Rotator.OnItemChange += UpdateUI;
    }

    private void OnDisable()
    {
        Rotator.OnItemChange -= UpdateUI;
    }

    private void UpdateUI(Item item)
    {
        if(item == null)
        {
            AcceptButton.gameObject.SetActive(false);
            RejectButton.gameObject.SetActive(false);
            ItemName.text = string.Empty;
            ItemDescription.text = string.Empty;
            return;
        }
        
        AcceptButton.gameObject.SetActive(true);
        RejectButton.gameObject.SetActive(true);
        ItemName.text = item.Data.Name;
        ItemDescription.text = item.Data.Description;
    }
}
