using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Vector3 startPos;
    public Quaternion startRot;

    private void Awake() {
        ToggleActive(false);
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void ToggleActive(bool active) {
        foreach (Item item in items)
            item.ToggleCollider(active);
    }
}
