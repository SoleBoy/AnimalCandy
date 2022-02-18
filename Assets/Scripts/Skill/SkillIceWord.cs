using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillIceWord : MonoBehaviour
{
    private Transform iceWord;
    private GameObject snowPrefab;

    private Collider boxCollider;
    private SkillItem item_skill;

    private Vector3 initScale;
    private Vector3 snowScale;
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        boxCollider = GetComponent<Collider>();

        iceWord = transform.Find("Cube");
        snowPrefab = transform.Find("xh").gameObject;
        snowScale = snowPrefab.transform.localScale;
        initScale = iceWord.localScale;
    }

    public void SetInit(SkillItem item, float hurt)
    {
        GetComponent<SkillHurt>().SetInit(item,hurt);
        boxCollider.enabled = false;
        iceWord.localScale = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            CreateSnow();
        }
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.PlaySource("bgwind_1", source);
        iceWord.DOScale(initScale, 0.2f);
        boxCollider.enabled = true;
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }
    private void CreateSnow()
    {
        var snow = Instantiate(snowPrefab);//ObjectPool.Instance.CreateObject("skill12SNow",snowPrefab);
        snow.SetActive(true);
        snow.transform.SetParent(transform);
        snow.transform.localPosition = new Vector3(Random.Range(-5,5), Random.Range(20, 30), Random.Range(5, 70));
        snow.transform.localScale = snowScale*Random.Range(0.5f,2f);
        snow.transform.DOLocalMoveY(-5, 6);
        //ObjectPool.Instance.CollectObject(snow,3);
    }
}
