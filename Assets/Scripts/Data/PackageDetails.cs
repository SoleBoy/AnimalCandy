using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageDetails : ScriptableObject
{
    public List<DetailsItem> items;
    public Dictionary<string, DetailsItem> dicItem = new Dictionary<string, DetailsItem>();
    public Dictionary<string, DetailsItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
