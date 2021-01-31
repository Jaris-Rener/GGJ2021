using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas boxCanvas;
    [SerializeField] private Canvas itemCanvas;

    public static UIManager instance;

    private void Awake() {
        instance = this;
    }

    public void ToggleBoxCanvas(bool active) => boxCanvas.enabled = active;
    public void ToggleItemCanvas(bool active) => itemCanvas.enabled = active;
}
