using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 技能滚动铁球
/// </summary>
public class SkillIronBall : MonoBehaviour
{
    Transform black_ball;
    Transform black_road;
    Transform black_floor;
    GameObject pieces;
    float lastTime;
    bool isFloor;
    float floorZ;
    float piecTime;
    bool isPieces;
    Color color;
    Color roabColor;
    AudioSource source;
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        pieces = transform.Find("PixelBlock").gameObject;
        black_ball = transform.Find("black_ball");
        black_road = transform.Find("black_road");
        black_floor = transform.Find("black_floor");
        color = black_ball.GetComponent<Renderer>().material.color;
        color.a = 1;
        roabColor = black_road.GetComponent<Renderer>().material.color;
        roabColor.a = 1;
    }
    public void SetInit(SkillItem item,float hurt)
    {
        floorZ = 2;
        lastTime = 1;
        black_ball.GetComponent<SkillHurt>().SetInit(item,hurt);
        black_ball.localPosition = new Vector3(0, 2, -15f);
        StartCoroutine(IronBallAnim());
    }
    void Update()
    {
        if(isFloor)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 0.04f && floorZ <= 90)
            {
                lastTime = 0;
                CloneFloor();
                if(floorZ >= 90)
                {
                    isFloor = false;
                }
            }
        }
        if (isPieces)
        {
            piecTime += Time.deltaTime;
            if (piecTime >= 0.1f)
            {
                piecTime = 0;
                BallPiecces();
            }
        }
    }

    IEnumerator IronBallAnim()
    {
        black_ball.DOLocalMoveZ(2,0.5f);
        yield return new WaitForSeconds(0.4f);
        AudioManager.Instance.PlaySource("skill_9_1", source);
        isFloor = true; isPieces = true;
        black_ball.DOLocalMoveZ(90, 2);
        black_road.DOLocalMoveZ(50,2);
        black_road.DOScaleZ(100,2);
        yield return new WaitForSeconds(2f);
        isPieces = false;
        black_ball.DOLocalMoveY(45,1f);
        black_road.GetComponent<Renderer>().material.DOFade(0,2);
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
        //black_road.localPosition=-Vector3.up*0.8f;
        //black_road.localScale=new Vector3(9,1,0);
        //black_road.GetComponent<Renderer>().material.color= roabColor;
        //ObjectPool.Instance.Clear("SkillIronBall");
        //ObjectPool.Instance.CollectObject(gameObject);
    }
    AudioSource audioSource;
    void CloneFloor()
    {
        float ran = Random.Range(1f, 3f);
        float ranY = Random.Range(10, 30);
        for (int i = 0; i < 2; i++)
        {
            var go = Instantiate(black_floor);//ObjectPool.Instance.CreateObject(black_floor.name, black_floor.gameObject).transform;
            go.SetParent(transform);
            go.gameObject.SetActive(true);
            if (i == 0)
            {
                go.localPosition = new Vector3(4, 0, floorZ);
                go.localEulerAngles = new Vector3(0,0,-30);
            }
            else
            {
                go.localPosition = new Vector3(-4, 0, floorZ);
                go.localEulerAngles = new Vector3(0, 0,30);
            }
            go.GetComponent<Renderer>().material.color= roabColor;
            go.localScale = new Vector3(ran,0, ran);
            floorZ += ran;
            StartCoroutine(FloorAnim(go,ranY));
            if(i==1)
            {
                if (!go.GetComponent<AudioSource>())
                {
                    audioSource = go.gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                }
                else
                {
                    audioSource = go.gameObject.GetComponent<AudioSource>();
                }
                AudioManager.Instance.PlaySource("skill_9_2", audioSource);
            }
        }
    }

    IEnumerator FloorAnim(Transform floor,float size)
    {
        floor.DOScaleY(size,0.05f);
        yield return new WaitForSeconds(0.1f);
        floor.DOScaleY(size*1.1f, 0.5f);
        yield return new WaitForSeconds(1f);
        floor.DOScaleY(0, 1);
        floor.GetComponent<Renderer>().material.DOFade(0,1);
        yield return new WaitForSeconds(1f);
        floor.gameObject.SetActive(false);
    }

    void BallPiecces()
    {
        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(pieces);//ObjectPool.Instance.CreateObject("SkillIronBall", pieces.gameObject);
            go.gameObject.SetActive(true);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one * Random.Range(0.2f,0.5f);
            go.GetComponent<Renderer>().material.color = color;
            go.transform.Rotate(Vector3.forward * Random.Range(-20, 20));
            Vector3 point = black_ball.localPosition;
            point.y -= 0.5f;
            point.x += Random.Range(-0.3f,0.3f);
            go.transform.localPosition = point;
            go.transform.GetComponent<PixelBlock>().SetPower(Random.Range(25,45));
        }
    }
}
