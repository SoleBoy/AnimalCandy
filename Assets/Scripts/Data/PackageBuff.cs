using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageBuff : ScriptableObject
{
    public List<BuffItem> items;
    public Dictionary<string, BuffItem> dicItem = new Dictionary<string, BuffItem>();
    public Dictionary<string, BuffItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
