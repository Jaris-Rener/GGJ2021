using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneMenu : MonoBehaviour {
    public bool isPhoneOpen = false;
    public GameObject Apps, Credits, Messages;
    public Transform PhoneOpen, PhoneClose;
    public MessageHandler MessageHandler;

    private Message _curMessage;

    public TextMeshProUGUI MessageText;
    public TextMeshProUGUI MessageName;
    public Image MessagePortrait;

    void Start() {
        Messages.transform.localScale = new Vector3(0, 0, 1);
        Credits.transform.localScale = new Vector3(0, 0, 1);
        Apps.SetActive(true);
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
        _curMessage = MessageHandler.GenerateRequest();
        SetMessage(_curMessage);
    }

    private void SetMessage(Message message)
    {
        MessageText.text = message.Text;
        MessageName.text = message.Sender;
        MessagePortrait.sprite = message.Portrait;
    }

    public void GoToGallery() {
        Debug.Log("Going to gallery");
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
}
