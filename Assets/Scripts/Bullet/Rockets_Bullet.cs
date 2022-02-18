using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockets_Bullet : MonoBehaviour
{
    private AudioSource source;
    private BoxCollider boxCollider;

    private GameObject particle;
    private List<GameObject> squibs = new List<GameObject>();

    private bool isCreate;
    private void Awake()
    {
        particle = transform.Find("TallExplosion").gameObject;
        if (!source) source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }
    public void OpenAnimal(float distance)
    {
        if (!boxCollider)
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(1.5f,8, distance);
            boxCollider.center = new Vector3(0,0, -distance * 0.5f);
        }
        squibs.Clear();
        boxCollider.enabled = false;
        isCreate = true;
        StartCoroutine(Animal());
    }

    public void StopAnimal()
    {
        isCreate = false;
        boxCollider.enabled = true;
        for (int i = 0; i < squibs.Count; i++)
        {
            squibs[i].SetActive(true);
            squibs[i].GetComponent<ParticleSystem>().Play();
        }
        AudioManager.Instance.PlaySource("skill_3_1", source);
        StartCoroutine(HideAnimal());
    }

    private IEnumerator HideAnimal()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < squibs.Count; i++)
        {
            squibs[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    private IEnumerator Animal()
    {
        yield return new WaitForSeconds(0.1f);
        var go = ObjectPool.Instance.CreateObject("TallExplosion", particle);
        go.transform.SetParent(null);
        go.transform.localPosition = transform.position;
        go.transform.localScale = Vector3.one * 2;
        go.transform.localEulerAngles = particle.transform.localEulerAngles;
        squibs.Add(go);
        yield return new WaitForSeconds(0.3f);
        if (isCreate)
            StartCoroutine(Animal());
    }


}
