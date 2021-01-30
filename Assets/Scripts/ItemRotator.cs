using System;
using DG.Tweening;
using UnityEngine;
using Input = UnityEngine.Input;

public class ItemRotator
    : MonoBehaviour
{
    public Camera Camera;
    public Item[] CollectedObjects;
    public Item CurrentObject;
    public float RotSpeed = 1f;
    public float Drag = 1f;
    public float CameraZoom = 5;

    public event Action<Item> OnItemChange;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _dir;
    private float _cameraFov;

    private int _currentObjIndex;

    [ContextMenu("Next")]
    public void NextItem()
    {
        CurrentObject.MeshRenderer.material.DOFade(0.0f, 0.4f).SetEase(Ease.InQuart);
        CurrentObject.transform.DOMove(_layoutPositionLeft, 0.6f);

        ++_currentObjIndex;
        if (_currentObjIndex >= CollectedObjects.Length)
            _currentObjIndex = 0;
        
        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;
        _dir = Vector3.zero;
        
        CurrentObject.transform.position = _layoutPositionRight;

        CurrentObject.MeshRenderer.material.DOFade(1.0f, 0.4f).SetEase(Ease.InQuart);
        CurrentObject.transform.DOMove(_layoutPositionMid, 0.6f);

        OnItemChange?.Invoke(CurrentObject);
    }

    [ContextMenu("Previous")]
    public void PreviousItem()
    {
        transform.DOComplete();
        transform.DORotate(new Vector3(0, 360f / CollectedObjects.Length, 0), 1f).SetRelative(true);
        --_currentObjIndex;
        if (_currentObjIndex < 0)
            _currentObjIndex = CollectedObjects.Length - 1;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;
        _dir = Vector3.zero;

        OnItemChange?.Invoke(CurrentObject);
    }

    private void Awake()
    {
        SetupItems();
        _cameraFov = Camera.fieldOfView;
    }

    private Vector3 _layoutPositionLeft => transform.position + new Vector3(-0.7f, 0, -0.3f);
    private Vector3 _layoutPositionMid => transform.position;
    private Vector3 _layoutPositionRight => transform.position + new Vector3(0.7f, 0, -0.3f);

    [ContextMenu("Layout")]
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _dir = Vector3.zero;
            CurrentObject.transform.rotation = Quaternion.identity;
        }

        if (Input.GetMouseButtonDown(0))
            OnDragStart();

        if (Input.GetMouseButtonUp(0))
            OnDragEnd();

        if (Input.GetMouseButton(0))
        {
            OnDrag();
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, _cameraFov - CameraZoom, Time.smoothDeltaTime * 10f);
        }
        else
        {
            _dir = Vector3.Lerp(_dir, Vector3.zero, Time.smoothDeltaTime * Drag);
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, _cameraFov, Time.smoothDeltaTime * 10f);
        }

        CurrentObject.transform.Rotate(_dir * Time.smoothDeltaTime * RotSpeed, Space.World);
    }

    private void OnDrag()
    {
        _endPos = Input.mousePosition;
        _dir = _endPos - _startPos;
        _dir = Camera.transform.TransformVector(_dir);
        _dir = Quaternion.AngleAxis(90, -Camera.transform.forward) * _dir;
    }

    private void OnDragEnd()
    {
        _startPos = Vector3.zero;
        _endPos = Vector3.zero;
    }

    private void OnDragStart()
    {
        _startPos = Input.mousePosition;
    }
}
