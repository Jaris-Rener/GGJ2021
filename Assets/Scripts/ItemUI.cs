using DG.Tweening;
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
        AcceptButton.onClick.AddListener(AcceptItem);
    }

    private void AcceptItem()
    {
        var item = Rotator.CurrentObject;
        Rotator.SetItem(null);
        item.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);

        ItemManager.Instance.AddItem(item);
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
            ItemName.text = string.Empty;
            ItemDescription.text = string.Empty;
            return;
        }

        ItemName.text = item.Data.Name;
        ItemDescription.text = item.Data.Description;
    }
}
