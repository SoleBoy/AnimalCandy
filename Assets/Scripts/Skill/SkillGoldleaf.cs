using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillGoldleaf : MonoBehaviour
{
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }
    public void SetInit(SkillItem item,float hurt,float dely)
    {
        this.GetComponent<SkillHurt>().SetInit(item, hurt);
        transform.localScale = Vector3.zero;
        StartCoroutine(OpenAnimal(dely));
    }

    private IEnumerator OpenAnimal(float dely)
    {
        yield return new WaitForSeconds(dely);
        AudioManager.Instance.PlaySource("detdiamond_1", source);
        transform.DOScale(Vector3.one,0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOScale(Vector3.one*1.1f, 1f);
        yield return new WaitForSeconds(1);
        transform.DOScale(Vector3.one * 1.2f, 1f);
        transform.DOLocalMoveY(25,1);
        yield return new WaitForSeconds(1);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 5);
    }
}
