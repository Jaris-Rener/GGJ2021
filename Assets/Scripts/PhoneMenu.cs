using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneMenu : MonoBehaviour {
    public bool isPhoneOpen = false;
    public GameObject Apps, Credits, Messages;
    public Transform PhoneOpen, PhoneClose;

    void Start() {
        Apps.SetActive(true);
        Credits.SetActive(false);
        Messages.SetActive(false);
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
        Debug.Log("Starting the game");
    }

    public void GoToGallery() {
        Debug.Log("Going to gallery");
    }

    public void GoToMessages() {
        Apps.SetActive(false);
        Messages.SetActive(true);
    }

    public void GoToCredits() {
        Apps.SetActive(false);
        Credits.SetActive(true);
    }

    public void ReturnToMenu() {
        Apps.SetActive(true);
        Credits.SetActive(false);
        Messages.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
