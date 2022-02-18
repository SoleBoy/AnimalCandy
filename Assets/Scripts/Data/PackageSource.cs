using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSource : ScriptableObject
{
    public List<SourceItem> items;
    public List<string> dicItem = new List<string>();
    public List<string> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].source);
        }
        return dicItem;
    }
}
