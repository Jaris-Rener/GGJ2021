using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneMenu : MonoBehaviour {
    public bool isPhoneOpen = false;
    public GameObject Apps, Credits, Messages;
    public Transform PhoneOpen, PhoneClose;
    public MessageHandler MessageHandler;

    public Message CurMessage;

    public TextMeshProUGUI MessageText;
    public TextMeshProUGUI MessageName;
    public Image MessagePortrait;

    public Button MessageBackButton;
    public Button PhoneCloseButton;

    void Start()
    {
        isPhoneOpen = true;
        transform.localPosition = PhoneOpen.localPosition;
        PhoneClose.gameObject.SetActive(false);
        Messages.transform.localScale = new Vector3(0, 0, 1);
        Credits.transform.localScale = new Vector3(0, 0, 1);
        Apps.SetActive(true);
        ScreenFader.Instance.SetFocusDistance(0.1f, 0);
    }

    public void HidePhone()
    {
        isPhoneOpen = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPhoneOpen = !isPhoneOpen;
        }

        if (isPhoneOpen) {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, PhoneOpen.localPosition, 0.03f);
        } else {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, PhoneClose.localPosition, 0.03f);
        }
    }

    public void StartGame() {
        GoToMessages();
        transform.DOComplete();
        transform.DOPunchRotation(new Vector3(0, 0, 7.5f), 0.135f);
        CurMessage = MessageHandler.GenerateRequest();
        SetMessage(CurMessage);
        MessageBackButton.gameObject.SetActive(false);
        PhoneCloseButton.gameObject.SetActive(true);
        ScreenFader.Instance.SetFocusDistance(6, 1);
    }

    public void SetMessage(Message message)
    {
        MessageText.text = message.Text;
        MessageName.text = message.Sender;
        MessagePortrait.sprite = message.Portrait;
    }

    public void GoToGallery() {
        SceneController.Instance.LoadScene("Gallery");
    }

    public void GoToMessages() {
        Messages.transform.localScale = new Vector3(0, 0, 1);
        Messages.transform.DOComplete();
        Messages.transform.DOScale(1, 0.15f);
    }

    public void GoToCredits() {
        Credits.transform.localScale = new Vector3(0, 0, 1);
        Credits.transform.DOComplete();
        Credits.transform.DOScale(1, 0.15f);
    }

    public void ReturnToMenu() {
        Credits.transform.DOComplete();
        Credits.transform.DOScale(0, 0.15f);
        Messages.transform.DOComplete();
        Messages.transform.DOScale(0, 0.15f);
    }

    public void QuitGame() {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void Show()
    {
        isPhoneOpen = true;
    }
}
