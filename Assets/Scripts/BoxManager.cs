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

    private Vector3 startBoxPos;
    private Vector3 startBoxRot;

    private Vector3 startItemPos;
    private Quaternion startItemRot;

    public Transform inspectTF;
    public Transform itemTF;
    public ItemRotator rotator;

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

        startBoxPos = box.transform.position;
        startBoxRot = box.transform.localEulerAngles;

        box.ToggleActive(true);

        box.transform.DOMove(inspectTF.position, moveTime);
        box.transform.DORotate(inspectTF.localEulerAngles, moveTime);
    }

    public void ReturnBox() {
        boxes[activeIndex].ToggleActive(false);

        boxes[activeIndex].transform.DOMove(startBoxPos, moveTime);
        boxes[activeIndex].transform.DORotate(startBoxRot, moveTime);
    }

    public void SetActiveItem(Item item)
    {
        startItemPos = item.transform.localPosition;
        startItemRot = item.transform.rotation;

        selectedItem = item;
        rotator.SetItem(item);

        item.transform.DOMove(itemTF.position, moveTime);
        item.transform.DORotateQuaternion(itemTF.rotation, moveTime);
    }

    public void ReturnItem()
    {
        rotator.SetItem(null);
        selectedItem.transform.DOLocalMove(startItemPos, moveTime);
        selectedItem.transform.DORotateQuaternion(startItemRot, moveTime);
    }

    //assigns items to boxes
    private void GenerateBoxItems() {

    }
}
