using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillLandslide : MonoBehaviour
{
    private Vector3 stonePoint;
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        stonePoint = transform.localPosition;
        if (GameManager.Instance.modeSelection == "roude")
        {
            stonePoint.z = 30;
        }
        else
        {
            stonePoint.z = -2;
        }
    }

    public void SetInit(SkillItem item,float hurt,float dely)
    {
        stonePoint.x = Random.Range(-4,4);
        transform.localPosition = stonePoint;
        GetComponent<SkillHurt>().SetInit(item,hurt);
        AudioManager.Instance.PlaySource("zctx_04032", source);
        StartCoroutine(OpenAnimal(dely));
    }

    private IEnumerator OpenAnimal(float index)
    {
        yield return new WaitForSeconds(index * 0.6f);
        transform.DOLocalMoveZ(90,5);
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
        //transform.gameObject.SetActive(false);
    }

    //private void Shuffle()
    //{
    //    for (int i = 0; i < arrayRan.Length; i++)
    //    {
    //        int temp = arrayRan[i];
    //        int randomIndex = Random.Range(0, arrayRan.Length);
    //        arrayRan[i] = arrayRan[randomIndex];
    //        arrayRan[randomIndex] = temp;
    //    }
    //}

    private void Update()
    {
        transform.Rotate(Vector3.right * 5, Space.World);
    }
}
