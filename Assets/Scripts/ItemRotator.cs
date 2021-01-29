using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = UnityEngine.Input;

public class ItemRotator
    : MonoBehaviour
{
    public Camera Camera;
    public GameObject[] CollectedObjects;
    public GameObject CurrentObject;
    public float RotSpeed = 1f;
    public float Drag = 1f;
    public float CameraZoom = 5;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _dir;
    private float _cameraFov;

    private int _currentObjIndex;

    [ContextMenu("Next")]
    public void NextItem()
    {
        transform.DOComplete();
        transform.DORotate(new Vector3(0, -360f/CollectedObjects.Length, 0), 1f).SetRelative(true);
        ++_currentObjIndex;
        if (_currentObjIndex >= CollectedObjects.Length)
            _currentObjIndex = 0;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;
        _dir = Vector3.zero;
    }
    
    [ContextMenu("Previous")]
    public void PreviousItem()
    {
        transform.DOComplete();
        transform.DORotate(new Vector3(0, 360f/CollectedObjects.Length, 0), 1f).SetRelative(true);
        --_currentObjIndex;
        if (_currentObjIndex < 0)
            _currentObjIndex = CollectedObjects.Length;

        CurrentObject = CollectedObjects[_currentObjIndex];
        CurrentObject.transform.rotation = Quaternion.identity;
        _dir = Vector3.zero;
    }

    private void Start()
    {
        SetupItems();
        _cameraFov = Camera.fieldOfView;
    }

    [ContextMenu("Layout")]
    private void SetupItems()
    {
        for (var i = 0; i < CollectedObjects.Length; i++)
        {
            var obj = CollectedObjects[i];
            var f = ((float)i/CollectedObjects.Length)*Mathf.PI*2;
            obj.transform.position = transform.position + new Vector3(Mathf.Sin(f), 0, Mathf.Cos(f))*5f;
            obj.transform.SetParent(transform);
        }

        CurrentObject = CollectedObjects[0];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
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
