using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageEnemy : ScriptableObject
{
    public List<EnemyItem> items;
    public Dictionary<string, EnemyItem> dicItem = new Dictionary<string, EnemyItem>();
    public Dictionary<string, EnemyItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
