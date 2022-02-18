using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillCookie : MonoBehaviour
{
    private Transform bear_Object;
    private GameObject ball_prefab;
    private ParticleSystem particle;
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        bear_Object = transform.Find("Bear");
        ball_prefab = transform.Find("Sphere").gameObject;
        particle = transform.Find("MagicAuraYellow").GetComponent<ParticleSystem>();
    }

    public void SetInit(SkillItem item,float hurt)
    {
        GetComponent<SkillHurt>().SetInit(item,hurt);
        bear_Object.localPosition = Vector3.up * 35;
        particle.transform.localScale = Vector3.zero;
        StartCoroutine(OpenAnimal());
    }

    private IEnumerator OpenAnimal()
    {
        particle.transform.DOScale(Vector3.one * 4,0.5f);
        particle.Play();
        bear_Object.DOLocalMoveY(3,0.5f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySource("skill_8_1", source);
        for (int i = 0; i < 7; i++)
        {
            StartCoroutine(CreateBubble(i*0.5f));
        }
        yield return new WaitForSeconds(4f);
        bear_Object.DOLocalMoveY(35, 1f);
        particle.transform.DOScale(Vector3.zero, 0.5f);
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
        //particle.Stop();
    }

    private IEnumerator CreateBubble(float dely)
    {
        yield return new WaitForSeconds(dely);
        var bubble = Instantiate(ball_prefab);//ObjectPool.Instance.CreateObject("skill17bubble",ball_prefab);
        Material material = bubble.GetComponent<Renderer>().material;
        material.color = Color.yellow;
        bubble.transform.SetParent(transform);
        bubble.transform.localScale = Vector3.zero;
        bubble.transform.localPosition = new Vector3(Random.Range(-2,3),0,-1f);
        yield return new WaitForSeconds(0.2f);
        bubble.transform.DOScale(Vector3.one*2,0.2f);
        material.DOFade(0.6f,0.2f);
        yield return new WaitForSeconds(0.2f);
        bubble.transform.DOScale(Vector3.one * 3, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bubble.transform.DOLocalMoveY(35,1);
        material.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        bubble.SetActive(false);
    }
}
