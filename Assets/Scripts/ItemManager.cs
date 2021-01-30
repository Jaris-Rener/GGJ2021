using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[DefaultExecutionOrder(-15)]
public class ItemManager
    : Singleton<ItemManager>
{
    public List<string> CollectedItemNames = new List<string>();
    private string _path => Application.persistentDataPath + "/items.json";

    private void Awake()
    {
        if(!File.Exists(_path))
            return;

        var json = File.ReadAllText(_path);
        CollectedItemNames = JsonConvert.DeserializeObject<List<string>>(json);
    }

    public void AddItem(Item item)
    {
        CollectedItemNames.Add(item.Data.Name);

        var json = JsonConvert.SerializeObject(CollectedItemNames);
        File.WriteAllText(_path, json);
    }
}