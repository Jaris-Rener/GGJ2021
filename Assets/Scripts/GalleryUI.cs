using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryUI
    : MonoBehaviour
{
    public ItemRotator Rotator;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public Button BackButton;

    private void Start()
    {
        BackButton.onClick.AddListener(ReturnToMain);
        UpdateUI(Rotator.CurrentObject);
    }

    private void ReturnToMain()
    {
        SceneController.Instance.LoadScene("Garage");
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
