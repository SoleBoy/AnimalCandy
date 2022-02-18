using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    private static EffectGenerator instance;
    public static EffectGenerator Instance { get => instance; }

    private GameObject pixelBlock;
    private GameObject fitEffects;
    private GameObject ghostPrefab;
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        pixelBlock = transform.Find("PixelBlock").gameObject;
        fitEffects = Resources.Load<GameObject>("Effects/Colour");
        //fitEffects[1] = Resources.Load<GameObject>("Effects/Fit/Colour2");
        //fitEffects[2] = Resources.Load<GameObject>("Effects/Fit/Colour4");
        ghostPrefab = Resources.Load<GameObject>("Effects/GhostEnemy");
    }
    public void RemoveGhost(Transform enemy)
    {
        var go = ObjectPool.Instance.CreateObject(ghostPrefab.name, ghostPrefab);
        go.transform.SetParent(transform);
        go.transform.position = enemy.position;
    }

    public void EenemyPixel(Transform enemy, int ID,int count)
    {
        Vector2 vector = enemy.GetComponent<AnimalControl>().sizeScale.GetComponent<Renderer>().bounds.size;
        int ran = UnityEngine.Random.Range(count-5, count);
        for (int i = 0; i < ran; i++)
        {
            StartCoroutine(SputterCube(vector, enemy.position, i * 0.02f, ExcelTool.Instance.animalColor[ID]));
        }
    }
    IEnumerator SputterCube(Vector2 size, Vector3 pos, float day, Color color)
    {
        yield return new WaitForSeconds(day);
        var go = ObjectPool.Instance.CreateObject(pixelBlock.name, pixelBlock.gameObject);
        go.transform.SetParent(transform);
        go.transform.localScale = Vector3.one * Random.Range(0.1f,0.3f);
        go.GetComponent<Renderer>().material.color = color;
        pos.x += Random.Range(-size.x * 0.5f, size.x * 0.5f);
        pos.y += size.y;
        go.transform.localPosition = pos;
        go.transform.Rotate(Vector3.up * UnityEngine.Random.Range(1, 30));
        go.GetComponent<SmallEffect>().SetPower(size.y+3);
    }

    public void Meage(Transform point)
    {
        var colour = ObjectPool.Instance.CreateObject("fiteffects", fitEffects);
        colour.transform.SetParent(transform);
        colour.transform.localPosition = point.position;
        colour.transform.localScale = point.localScale*0.015f;
        colour.GetComponent<ParticleSystem>().Play();
        StartCoroutine(HideMeger(colour));
    }
    IEnumerator HideMeger(GameObject go)
    {
        yield return new WaitForSeconds(2);
        ObjectPool.Instance.CollectObject(go);
    }
}
