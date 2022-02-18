using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePackage : ScriptableObject
{
    public List<LineItem> items;
    public Dictionary<string, LineItem> dicItem = new Dictionary<string, LineItem>();
    public Dictionary<string, LineItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
