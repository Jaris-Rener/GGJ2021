using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public MeshRenderer MeshRenderer;
    public Collider Collider;

    public void ToggleCollider(bool active) => Collider.enabled = active;
}