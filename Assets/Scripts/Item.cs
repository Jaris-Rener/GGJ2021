using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public MeshRenderer MeshRenderer;
    public Collider Collider;

    public Vector3 StartPosLocal;
    public Quaternion StartRotLocal;

    private void Awake()
    {
        StartPosLocal = transform.localPosition;
        StartRotLocal = transform.localRotation;
    }

    public void ToggleCollider(bool active) => Collider.enabled = active;
}