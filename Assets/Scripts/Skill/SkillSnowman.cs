using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillSnowman : MonoBehaviour
{
    private Transform snowMan;
    private Transform water;
    private Transform redHeart;

    private Vector3 snowPoint;
    private Vector3 waterPoint;
    private AudioSource source;
    public float heartNum;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        snowMan = transform.Find("snowman1");
        water = transform.Find("Water");
        redHeart = transform.Find("snowman1/RedHeart");

        snowPoint = new Vector3(-0.5f,35,45);
        waterPoint = new Vector3(0,-10,175);
    }

    public void SetInit(float hurt)
    {
        heartNum = hurt;
        snowMan.localPosition = snowPoint;
        water.localPosition = waterPoint;
        snowMan.GetComponent<Collider>().enabled = true;
        StartCoroutine(OpenAnimal());
        StartCoroutine(DelayMove());
        AudioManager.Instance.PlaySource("bgrain_1", source);
    }

    private IEnumerator OpenAnimal()
    {
        water.DOLocalMoveY(5,3);
        snowMan.DOLocalMoveY(4,1);
        yield return new WaitForSeconds(2f);
        snowMan.DOLocalMoveZ(-55, 5);
        yield return new WaitForSeconds(3f);
        water.DOLocalMoveY(-5, 2);
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    IEnumerator DelayMove()
    {
        redHeart.DOLocalMoveY(0.045f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        redHeart.DOLocalMoveY(0.035f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayMove());
    }
}
