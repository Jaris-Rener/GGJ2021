using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxManager : MonoBehaviour
{
    public Box[] boxes;
    public Item[] items;

    public float moveTime = 1f;

    private int activeIndex;
    private Item selectedItem;

    public Transform inspectTF;
    public Transform itemTF;
    public ItemRotator rotator;
    public ParticleSystem selectParticles;
    public PhoneMenu phone;

    public static BoxManager instance;

    private void Awake() {
        instance = this;
        GenerateBoxItems();
    }

    public void SetActiveBox(Box box) {
        for(int i = 0, iMax = boxes.Length; i < iMax; i++) {
            if (box == boxes[i])
                activeIndex = i;
        }

        if (phone.isPhoneOpen)
            phone.HidePhone();

        box.ToggleActive(true);

        box.transform.DOMove(inspectTF.position, moveTime);
        box.transform.DORotate(inspectTF.localEulerAngles, moveTime);
    }

    public void ReturnBox() {
        boxes[activeIndex].ToggleActive(false);
        
        boxes[activeIndex].transform.DOMove(boxes[activeIndex].startPos, moveTime);
        boxes[activeIndex].transform.DORotate(boxes[activeIndex].startRot.eulerAngles, moveTime);
    }

    public void SetActiveItem(Item item)
    {
        ScreenFader.Instance.SetFocusDistance(0.245f);
        selectedItem = item;
        rotator.SetItem(item);

        item.transform.DOMove(itemTF.position, moveTime);
        item.transform.DORotateQuaternion(itemTF.rotation, moveTime);
    }

    public void ReturnItem()
    {
        ScreenFader.Instance.SetFocusDistance(6f);
        rotator.SetItem(null);
        selectedItem.transform.DOLocalMove(selectedItem.StartPosLocal, moveTime);
        selectedItem.transform.DOLocalRotateQuaternion(selectedItem.StartRotLocal, moveTime);
    }

    public void SelectItem()
    {
        StartCoroutine(Coro());

        IEnumerator Coro()
        {
            rotator.SetItem(null);
            UIManager.instance.ToggleItemCanvas(false);
            UIManager.instance.ToggleBoxCanvas(false);
            selectedItem.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
            selectParticles.Play();
            ItemManager.Instance.AddItem(selectedItem);
            yield return new WaitForSeconds(1f);
            ReturnBox();
            GarageManager.instance.ResetState();
            yield return new WaitForSeconds(2f);
            selectedItem.ResetAll();
            phone.SetMessage(phone.MessageHandler.GenerateResponse(phone.CurMessage,
                selectedItem.Data.Tags.Contains(phone.CurMessage.Tag)));
            phone.MessageBackButton.gameObject.SetActive(true);
            phone.PhoneCloseButton.gameObject.SetActive(false);
            phone.Show();
        }
    }

    //assigns items to boxes
    private void GenerateBoxItems() {

    }
}
