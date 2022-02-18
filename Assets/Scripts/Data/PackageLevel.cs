using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLevel : ScriptableObject
{
    public List<LevelItem> items;
    public Dictionary<string, LevelItem> dicItem = new Dictionary<string, LevelItem>();
    public Dictionary<string, LevelItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
