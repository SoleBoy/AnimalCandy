using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSeafire : MonoBehaviour
{
    private Transform seaFire;
    private Vector3 initScale;
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        seaFire = transform.Find("Skill13");
        initScale = seaFire.localScale;
    }

    public void SetInit(SkillItem item, float hurt)
    {
        seaFire.localPosition = Vector3.forward * 90;
        seaFire.localScale = Vector3.zero;
        seaFire.GetComponent<SkillData>().SetInit(item,hurt);
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(0.1f);
        seaFire.DOScale(initScale, 0.2f);
        seaFire.DOLocalMoveZ(48,0.2f);
        AudioManager.Instance.PlaySource("fireweather_1", source);
        yield return new WaitForSeconds(7f);
        seaFire.DOScale(Vector3.zero, 0.5f);
        seaFire.DOLocalMoveZ(90, 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
