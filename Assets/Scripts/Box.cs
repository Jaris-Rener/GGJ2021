using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake() {
        ToggleActive(false);
    }

    public void ToggleActive(bool active) {
        foreach (Item item in items)
            item.ToggleCollider(active);
    }
}
