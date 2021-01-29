using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData
    : ScriptableObject
{
    public string Name;
    public List<ItemTag> Tags;
}