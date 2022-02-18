using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SkillRedwind : MonoBehaviour
{
    private ParticleSystem wind_red;
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        wind_red = transform.Find("Cyclone").GetComponent<ParticleSystem>();
    }

    public void SetInit(SkillItem item, float hurt)
    {
        AudioManager.Instance.PlaySource("windweather_1", source);
        GetComponent<SkillHurt>().SetInit(item, hurt);
        wind_red.Play();
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        transform.DOLocalMoveZ(90,5);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        //wind_red.Stop();
        //gameObject.SetActive(false);
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * 10);
    }

}
