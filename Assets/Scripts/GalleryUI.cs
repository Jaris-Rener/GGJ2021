using TMPro;
using UnityEngine;

public class GalleryUI
    : MonoBehaviour
{
    public ItemRotator Rotator;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;

    private void Start()
    {
        UpdateUI(Rotator.CurrentObject);
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
            return;

        ItemName.text = item.Data.Name;
        ItemDescription.text = item.Data.Description;
    }
}
