using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTask : ScriptableObject
{
    public List<MissionItem> items;
    public Dictionary<string, MissionItem> dicItem = new Dictionary<string, MissionItem>();
    public Dictionary<string, MissionItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
