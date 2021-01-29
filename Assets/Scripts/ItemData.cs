using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData
    : ScriptableObject
{
    public string Name;
    [TextArea(2, 4)]
    public string Description;
    public List<ItemTag> Tags;
}