using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemGallery
    : MonoBehaviour
{
    public List<Item> CollectedObjects = new List<Item>();
    public Item CurrentObject;
    public ItemRotator Rotator;
    public Button NextButton;
    public Button PreviousButton;

    public event Action<Item> OnItemChange;

    private int _currentObjIndex;

    public void NextItem()
    {
        CurrentObject.MeshRenderer.material.DOFloat(0.0f, "_Alpha1", 0.4f).SetEase(Ease.OutQuart);
        CurrentObject.transform.DOMove(_layoutPositionLeft, 0.6f);

        ++_currentObjIndex;
        if (_currentObjIndex >= CollectedObjects.Count)
            _currentObjIndex = 0;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;

        CurrentObject.transform.position = _layoutPositionRight;

        CurrentObject.MeshRenderer.material.DOFloat(1.0f, "_Alpha1", 0.4f).SetEase(Ease.InQuart);
        CurrentObject.transform.DOMove(_layoutPositionMid, 0.6f);

        OnItemChange?.Invoke(CurrentObject);
        Rotator.SetItem(CurrentObject);
    }

    public void PreviousItem()
    {
        CurrentObject.MeshRenderer.material.DOFloat(0.0f, "_Alpha1", 0.4f).SetEase(Ease.OutQuart);
        CurrentObject.transform.DOMove(_layoutPositionRight, 0.6f);

        --_currentObjIndex;
        if (_currentObjIndex < 0)
            _currentObjIndex = CollectedObjects.Count - 1;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;

        CurrentObject.transform.position = _layoutPositionLeft;

        CurrentObject.MeshRenderer.material.DOFloat(1.0f, "_Alpha1", 0.4f).SetEase(Ease.InQuart);
        CurrentObject.transform.DOMove(_layoutPositionMid, 0.6f);

        OnItemChange?.Invoke(CurrentObject);
        Rotator.SetItem(CurrentObject);
    }

    private void Awake()
    {
        SpawnItems();
        SetupItems();

        if (CollectedObjects.Count <= 1)
        {
            NextButton.gameObject.SetActive(false);
            PreviousButton.gameObject.SetActive(false);
        }
    }

    private void SpawnItems()
    {
        var items = Resources.LoadAll<Item>("Items");
        print($"loading {items.Length} items");
        foreach (var itemName in ItemManager.Instance.CollectedItemNames)
        {
            var item = items.FirstOrDefault(x => x.Data.Name == itemName);
            if (item == null)
                continue;
            
            print($"spawning {itemName}");
            var obj = Instantiate(item, transform);
            obj.MeshRenderer.material.SetFloat("_PropFade", 0);
            CollectedObjects.Add(obj);
        }
    }

    private Vector3 _layoutPositionLeft => transform.position + new Vector3(-2f, 0, -0.3f);
    private Vector3 _layoutPositionMid => transform.position;
    private Vector3 _layoutPositionRight => transform.position + new Vector3(2f, 0, -0.3f);

    private void SetupItems()
    {
        if(CollectedObjects.Count <= 0)
            return;

        CollectedObjects[CollectedObjects.Count - 1].transform.position = _layoutPositionRight;
        CollectedObjects[0].transform.position = _layoutPositionMid;

        if (CollectedObjects.Count > 1)
            CollectedObjects[1].transform.position = _layoutPositionLeft;

        for (int i = 1; i < CollectedObjects.Count; i++)
        {
            CollectedObjects[i].MeshRenderer.material.SetFloat("_Alpha1", 0);
            CollectedObjects[i].transform.position = _layoutPositionLeft;
        }

        CurrentObject = CollectedObjects[0];
        Rotator.SetItem(CurrentObject);
    }
}
