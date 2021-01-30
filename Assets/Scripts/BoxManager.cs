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

    private Vector3 startPos;
    private Vector3 startRot;

    public Transform inspectTF;

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

        startPos = box.transform.position;
        startRot = box.transform.localEulerAngles;

        box.transform.DOMove(inspectTF.position, moveTime);
        box.transform.DORotate(inspectTF.localEulerAngles, moveTime);
    }

    public void ReturnBox() {
        boxes[activeIndex].transform.DOMove(startPos, moveTime);
        boxes[activeIndex].transform.DORotate(startRot, moveTime);
    }

    //assigns items to boxes
    private void GenerateBoxItems() {

    }
}
