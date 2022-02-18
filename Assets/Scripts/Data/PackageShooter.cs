using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageShooter :ScriptableObject
{
    public List<ShooterItem> items;
    public Dictionary<string, ShooterItem> dicItem = new Dictionary<string, ShooterItem>();
    public Dictionary<string, ShooterItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
