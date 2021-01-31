using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public MeshRenderer MeshRenderer;
    public Collider Collider;

    public Vector3 StartPosLocal;
    public Quaternion StartRotLocal;

    private Vector3 _startScale;

    private void Awake()
    {
        _startScale = transform.localScale;
        StartPosLocal = transform.localPosition;
        StartRotLocal = transform.localRotation;
    }

    public void ToggleCollider(bool active) => Collider.enabled = active;

    public void ResetAll()
    {
        transform.localPosition = StartPosLocal;
        transform.localRotation = StartRotLocal;
        transform.localScale = _startScale;
    }
}