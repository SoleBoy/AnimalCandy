using System.Collections.Generic;
using UnityEngine;

public class Modification
{
    public static void ServerData(List<Transform> animal,List<int> death)
    {
        string buildData = "";
        for (int i = 0; i < animal.Count; i++)
        {
            buildData += animal[i].GetComponent<AnimalControl>().indexAnimal + ",";
            buildData += animal[i].localPosition.x + "|";
            buildData += animal[i].localPosition.y + "|";
            buildData += animal[i].localPosition.z + ",";
            buildData += animal[i].localEulerAngles.x + "|";
            buildData += animal[i].localEulerAngles.y + "|";
            buildData += animal[i].localEulerAngles.z + ";";
        }
        string deathData = "";
        for (int i = 0; i < death.Count; i++)
        {
            deathData += death[i]+ ",";
        }
        PlayerPrefs.SetString("DeathData", deathData);
        PlayerPrefs.SetString("BuildData", buildData);
    }

    public static List<int> GetDeathData()
    {
        List<int> lists = new List<int>();
        string[] data = PlayerPrefs.GetString("DeathData").Split(',');
        for (int i = 0; i < data.Length-1; i++)
        {
            lists.Add(int.Parse(data[i]));
            UIBase.Instance.animalHead.SetHead(int.Parse(data[i]),-1);
        }
        return lists;
    }

    public static void GetPoints(Transform[] animal,Transform parent)
    {
        List<PointItem> pointItems = new List<PointItem>();
        string[] data = PlayerPrefs.GetString("BuildData").Split(';');
        for (int i = 0; i < data.Length - 1; i++)
        {
            PointItem item = new PointItem();
            string[] single = data[i].Split(',');
            item.ID = int.Parse(single[0]);
            for (int j = 1; j < single.Length; j++)
            {
                string[] point = single[j].Split('|');
                if(point.Length >= 3)
                {
                    if (j == 1)
                    {
                        item.point.x = float.Parse(point[0]);
                        item.point.y = float.Parse(point[1]);
                        item.point.z = float.Parse(point[2]);
                    }
                    else if (j == 2)
                    {
                        item.angle.x = float.Parse(point[0]);
                        item.angle.y = float.Parse(point[1]);
                        item.angle.z = float.Parse(point[2]);
                    }
                }
            }
            pointItems.Add(item);
        }

        for (int i = 0; i < pointItems.Count; i++)
        {
            var go = ObjectPool.Instance.CreateObject(animal[pointItems[i].ID].name,animal[pointItems[i].ID].gameObject).transform;
            go.SetParent(parent);
            go.localPosition = pointItems[i].point;
            go.localEulerAngles = pointItems[i].angle;
            go.localScale = Vector3.one * UIBase.Instance.animalHead.IsScale(pointItems[i].ID);
            go.GetComponent<AnimalControl>().SetData(pointItems[i].ID);
            go.GetComponent<AnimalControl>().FreedObject(true);
            go.GetComponent<AnimalControl>().JudeCollIsion();
            UIBase.Instance.animalHead.SetHead(pointItems[i].ID, -1);
            UIBase.Instance.bornAnimal.Add(go);
        }
    }
}

public class PointItem
{
    public int ID;
    public Vector3 point;
    public Vector3 angle;
}