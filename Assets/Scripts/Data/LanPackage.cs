using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanPackage : ScriptableObject
{
    public List<LanguageItem> items;
    public Dictionary<string, string> dicItem = new Dictionary<string, string>();
    public Dictionary<string, string> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].key, items[i].value);
        }
        return dicItem;
    }
}
