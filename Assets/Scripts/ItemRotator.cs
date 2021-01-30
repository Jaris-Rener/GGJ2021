using System;
using UnityEngine;

public class ItemRotator
    : MonoBehaviour
{
    public Item CurrentObject { get; private set; }

    public Camera Camera;
    public float RotSpeed = 1f;
    public float Drag = 1f;
    public float CameraZoom = 5;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _dir;
    private float _cameraFov;
    public event Action<Item> OnItemChange;

    private void Start()
    {
        _cameraFov = Camera.fieldOfView;
    }

    public void SetItem(Item item)
    {
        CurrentObject = item;
        OnItemChange?.Invoke(item);
    }

    private void Update()
    {
        if(CurrentObject == null)
            return;

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

    public void ZeroMovement()
    {
        CurrentObject.transform.rotation = Quaternion.identity;
        _dir = Vector3.zero;
    }
}