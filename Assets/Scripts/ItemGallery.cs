using System;
using DG.Tweening;
using UnityEngine;

public class ItemGallery
    : MonoBehaviour
{
    public Item[] CollectedObjects;
    public Item CurrentObject;
    public ItemRotator Rotator;

    public event Action<Item> OnItemChange;

    private int _currentObjIndex;

    public void NextItem()
    {
        CurrentObject.MeshRenderer.material.DOFade(0.0f, 0.4f).SetEase(Ease.OutQuart);
        CurrentObject.transform.DOMove(_layoutPositionLeft, 0.6f);

        ++_currentObjIndex;
        if (_currentObjIndex >= CollectedObjects.Length)
            _currentObjIndex = 0;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;

        CurrentObject.transform.position = _layoutPositionRight;

        CurrentObject.MeshRenderer.material.DOFade(1.0f, 0.4f).SetEase(Ease.InQuart);
        CurrentObject.transform.DOMove(_layoutPositionMid, 0.6f);

        OnItemChange?.Invoke(CurrentObject);
        Rotator.SetItem(CurrentObject);
    }

    public void PreviousItem()
    {
        CurrentObject.MeshRenderer.material.DOFade(0.0f, 0.4f).SetEase(Ease.OutQuart);
        CurrentObject.transform.DOMove(_layoutPositionRight, 0.6f);

        --_currentObjIndex;
        if (_currentObjIndex < 0)
            _currentObjIndex = CollectedObjects.Length - 1;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;

        CurrentObject.transform.position = _layoutPositionLeft;

        CurrentObject.MeshRenderer.material.DOFade(1.0f, 0.4f).SetEase(Ease.InQuart);
        CurrentObject.transform.DOMove(_layoutPositionMid, 0.6f);

        OnItemChange?.Invoke(CurrentObject);
        Rotator.SetItem(CurrentObject);
    }

    private void Awake()
    {
        SetupItems();
    }

    private Vector3 _layoutPositionLeft => transform.position + new Vector3(-0.7f, 0, -0.3f);
    private Vector3 _layoutPositionMid => transform.position;
    private Vector3 _layoutPositionRight => transform.position + new Vector3(0.7f, 0, -0.3f);

    private void SetupItems()
    {
        CollectedObjects[CollectedObjects.Length - 1].transform.position = _layoutPositionRight;
        CollectedObjects[0].transform.position = _layoutPositionMid;
        CollectedObjects[1].transform.position = _layoutPositionLeft;

        for (int i = 1; i < CollectedObjects.Length; i++)
        {
            CollectedObjects[i].MeshRenderer.material.color = new Color(1f, 1f, 1f, 0f);
        }

        CurrentObject = CollectedObjects[0];
        Rotator.SetItem(CurrentObject);
    }
}
