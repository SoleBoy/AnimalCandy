using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSkill : ScriptableObject
{
    public List<SkillItem> items;
    public Dictionary<string, SkillItem> dicItem = new Dictionary<string, SkillItem>();
    public Dictionary<string, SkillItem> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
