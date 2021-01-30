using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public MeshRenderer MeshRenderer;
    public Collider Collider;

    public Vector3 StartPosLocal;
    public Quaternion StartRot;

    private void Awake()
    {
        StartPosLocal = transform.localPosition;
        StartRot = transform.rotation;
    }

    public void ToggleCollider(bool active) => Collider.enabled = active;
}