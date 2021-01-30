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
        selectedItem = item;
        rotator.SetItem(item);

        item.transform.DOMove(itemTF.position, moveTime);
        item.transform.DORotateQuaternion(itemTF.rotation, moveTime);
    }

    public void ReturnItem()
    {
        rotator.SetItem(null);
        selectedItem.transform.DOLocalMove(selectedItem.StartPosLocal, moveTime);
        selectedItem.transform.DOLocalRotateQuaternion(selectedItem.StartRotLocal, moveTime);
    }

    public void SelectItem() {
        rotator.SetItem(null);
        selectedItem.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        selectParticles.Play();
        ItemManager.Instance.AddItem(selectedItem);
    }

    //assigns items to boxes
    private void GenerateBoxItems() {

    }
}
