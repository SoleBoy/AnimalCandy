using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能熔岩
/// </summary>
public class SkillMagma : MonoBehaviour
{
    Transform green_brick1;
    Transform green_brick2;
    Transform yellow_brick1;
    Transform yellow_brick2;
    Transform yellow_brick3;
    Transform yellow_brick4;

    Transform little_brick1;
    Transform spherePrefab;
    Transform smokePrefab;
    Transform ripple;
    Transform pices;
    bool isPices;
    float lastTime;
    AudioSource source;

    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        pices = transform.Find("pices");
        ripple = transform.Find("ripple");
        smokePrefab = transform.Find("smoke");
        spherePrefab = transform.Find("Sphere");
        little_brick1 = transform.Find("little_brick1");
        green_brick1 = transform.Find("green_brick1");
        green_brick2 = transform.Find("green_brick2");
        yellow_brick1 = transform.Find("yellow_brick1");
        yellow_brick2 = transform.Find("yellow_brick2");
        yellow_brick3 = transform.Find("yellow_brick3");
        yellow_brick4 = transform.Find("yellow_brick4");
       
    }
    public void SetInit(SkillItem item,float hurt)
    {
        green_brick1.transform.localPosition = new Vector3(80, 17, 55);
        green_brick2.transform.localPosition = new Vector3(-80, 17, 55);
        yellow_brick1.transform.localPosition = new Vector3(80, 17, 55);
        yellow_brick2.transform.localPosition = new Vector3(-80, 17, 55);
        yellow_brick3.GetComponent<SkillData>().SetInit(item,hurt);
        yellow_brick4.GetComponent<SkillData>().SetInit(item,hurt);
        StartCoroutine(MagmaAnim());
    }

    IEnumerator MagmaAnim()
    {
        green_brick1.DOLocalMove(new Vector3(19.7f,11.7f, 55), 0.2f);
        green_brick2.DOLocalMove(new Vector3(-19.7f,11.7f, 55), 0.2f);
        yellow_brick1.DOLocalMove(new Vector3(46.4f,43.3f,55),0.2f);
        yellow_brick2.DOLocalMove(new Vector3(-46.4f, 43.3f, 55), 0.2f);
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlaySource("skill_6_1", source);
        MagmaBrick(1); MagmaBrick(2);
        StartCoroutine(BubbleAnim(1)); ; StartCoroutine(BubbleAnim(2));
        MagmaSmoke(1); MagmaSmoke(2);
        yellow_brick1.DOLocalMove(new Vector3(17.4f, 14.3f, 55), 1.5f);
        yellow_brick2.DOLocalMove(new Vector3(-17.4f, 14.3f, 55), 1.5f);
        isPices = true;
        yield return new WaitForSeconds(1.2f);
        AudioManager.Instance.PlaySource("skill_6_2", source);
        yellow_brick3.DOLocalMoveX(2,1.5f);
        yellow_brick4.DOLocalMoveX(-2, 1.5f);
        yellow_brick3.DOScaleX(5, 1);
        yellow_brick4.DOScaleX(5, 1);
        MagmaSmoke(3); MagmaRipple();
        StartCoroutine(BubbleAnim(3));
        yield return new WaitForSeconds(2f);
        yellow_brick1.DOLocalMove(new Vector3(-14.6f, -17.7f, 55), 1.5f);
        yellow_brick2.DOLocalMove(new Vector3(14.6f, -17.7f, 55), 1.5f);
        isPices = false;
        yield return new WaitForSeconds(3f);
        yellow_brick3.DOLocalMoveX(5, 2);
        yellow_brick4.DOLocalMoveX(-5, 2);
        yellow_brick3.DOScaleX(0, 2);
        yellow_brick4.DOScaleX(0, 2);
        green_brick1.DOLocalMoveX(80, 1.5f);
        green_brick2.DOLocalMoveX(-80, 1.5f);
        yield return new WaitForSeconds(3);
        GameObject.Destroy(gameObject);
        //ObjectPool.Instance.CollectObject(gameObject);
        //ObjectPool.Instance.Clear(little_brick1.name);
        //ObjectPool.Instance.Clear(smokePrefab.name);
        //ObjectPool.Instance.Clear(spherePrefab.name);
        //ObjectPool.Instance.Clear(ripple.name);
        //ObjectPool.Instance.Clear(pices.name);
    }
    private void Update()
    {
        if(isPices)
        {
            lastTime += Time.deltaTime;
            if(lastTime >= 0.2f)
            {
                lastTime = 0;
                Piecces(5);
            }
        }
    }
    //小岩浆地块
    void MagmaBrick(int index)
    {
        for (int i = 0; i < 10; i++)
        {
            var brick = Instantiate(little_brick1);//ObjectPool.Instance.CreateObject(little_brick1.name,little_brick1.gameObject).transform;
            brick.gameObject.SetActive(true);
            brick.SetParent(transform);
            float ran = Random.Range(6, 80);
            if(index == 1)
            {
                brick.localEulerAngles = new Vector3(0,0,45);
                brick.localPosition = new Vector3(32.8f, 28.8f,ran);
                brick.DOLocalMove(new Vector3(-0.5f, -4f, ran),Random.Range(1.8f,2.2f));
            }
            else
            {
                brick.localEulerAngles = new Vector3(0, 0, -45);
                brick.localPosition = new Vector3(-32.8f, 28.8f, ran);
                brick.DOLocalMove(new Vector3(0.5f, -4f, ran),Random.Range(1.8f, 2.2f));
            }
            brick.localScale = new Vector3(8, 0.25f, Random.Range(1, 3));
            StartCoroutine(BrickAnim(brick));
        }
    }
    IEnumerator BrickAnim(Transform brick)
    {
        yield return new WaitForSeconds(2.5f);
        brick.gameObject.SetActive(false);
    }
    //烟雾
    void MagmaSmoke(int index)
    {
        for (int i = 0; i < 10; i++)
        {
            var smoke = Instantiate(smokePrefab);//ObjectPool.Instance.CreateObject(smokePrefab.name, smokePrefab.gameObject).transform;
            smoke.gameObject.SetActive(true);
            smoke.SetParent(transform);
            if(index == 1)
            {
                float ran = Random.Range(5, 25);
                smoke.localPosition = new Vector3(ran + 0.3f, ran, Random.Range(6, 80));
            }
            else if(index == 2)
            {
                float ran = Random.Range(5, 25);
                smoke.localPosition = new Vector3(-(ran + 0.3f), ran, Random.Range(6, 80));
            }
            else
            {
                smoke.localPosition = new Vector3(Random.Range(-4, 4), 2.6f, Random.Range(6, 80));
            }
            smoke.localScale = Vector3.zero;
            StartCoroutine(SmokeAnim(smoke));
        }
    }
    IEnumerator SmokeAnim(Transform smoke)
    {
        yield return new WaitForSeconds(0.6f);
        smoke.DOScale(Vector3.one * Random.Range(1f,3f), 3f);
        yield return new WaitForSeconds(4);
        smoke.gameObject.SetActive(false);
    }
    //泡泡
    IEnumerator BubbleAnim(int index)
    {
        yield return new WaitForSeconds(Random.Range(0.3f,1f));
        for (int i = 0; i < 20; i++)
        {
            var bubble = Instantiate(spherePrefab);//ObjectPool.Instance.CreateObject(spherePrefab.name, spherePrefab.gameObject).transform;
            bubble.gameObject.SetActive(true);
            bubble.SetParent(transform);
            if (index == 1)
            {
                float ran = Random.Range(2, 20);
                bubble.localPosition = new Vector3(ran + 2.6f, ran, Random.Range(6, 80));
            }
            else if (index == 2)
            {
                float ran = Random.Range(2, 20);
                bubble.localPosition = new Vector3(-(ran + 2.6f), ran, Random.Range(6, 80));
            }
            else
            {
                bubble.localPosition = new Vector3(Random.Range(-4, 4), 0f, Random.Range(6, 80));
                bubble.localScale = Vector3.zero;
            }
            bubble.localScale = Vector3.zero;
            StartCoroutine(Bubble(bubble));
        }
    }
    IEnumerator Bubble(Transform bubble)
    {
        bubble.DOScale(Vector3.one * Random.Range(0.8f,2f), 0.5f);
        yield return new WaitForSeconds(4f);
        bubble.DOLocalMoveY(25, 2);
        bubble.DOScale(bubble.localScale * Random.Range(1f, 1.5f), 2);
        bubble.GetComponent<Renderer>().material.DOFade(0, 2);
        yield return new WaitForSeconds(2);
        bubble.GetComponent<Renderer>().material.DOFade(1, 0);
        bubble.gameObject.SetActive(false);
    }
    //波纹
    void MagmaRipple()
    {
        for (int i = 0; i < 15; i++)
        {
            var go = Instantiate(ripple);//ObjectPool.Instance.CreateObject(ripple.name, ripple.gameObject).transform;
            go.gameObject.SetActive(true);
            go.SetParent(transform);
            go.localPosition = new Vector3(0,0, Random.Range(0, 60));
            go.localScale = Vector3.zero;
            StartCoroutine(RippleAnim(go));
        }
    }
    IEnumerator RippleAnim(Transform go)
    {
        yield return new WaitForSeconds(1f);
        go.localScale = Vector3.one*Random.Range(0.5f,1f);
        yield return new WaitForSeconds(4f);
        go.gameObject.SetActive(false);
    }

    //小碎块
    void Piecces(float max)
    {
        //float num = Random.Range(3, 5);
        float ran = Random.Range(2, 15);
        for (int i = 0; i < 3; i++)
        {
            var spray = Instantiate(pices);//ObjectPool.Instance.CreateObject(pices.name, pices.gameObject);
            spray.gameObject.SetActive(true);
            spray.transform.SetParent(pices.parent);
            spray.transform.eulerAngles = new Vector3(0,0,Random.Range(25,60));
            spray.transform.localScale = Vector3.one * Random.Range(0.5f, 0.9f);
            spray.transform.localPosition = new Vector3(ran + 2.6f, ran, Random.Range(6, 80));
            spray.transform.GetComponent<PixelBlock>().SetPower(Random.Range(10, 15));
        }
        for (int i = 0; i < 3; i++)
        {
            var spray = Instantiate(pices);//ObjectPool.Instance.CreateObject(pices.name, pices.gameObject);
            spray.gameObject.SetActive(true);
            spray.transform.SetParent(pices.parent);
            spray.transform.eulerAngles = new Vector3(0, 0, Random.Range(-60, -25));
            spray.transform.localScale = Vector3.one * Random.Range(0.5f, 0.9f);
            spray.transform.localPosition = new Vector3(-(ran + 2.6f), ran, Random.Range(6, 80));
            spray.transform.GetComponent<PixelBlock>().SetPower(Random.Range(10, 15));
        }
    }
}
