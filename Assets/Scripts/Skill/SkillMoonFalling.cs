using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 技能月亮坠落
/// </summary>
public class SkillMoonFalling : MonoBehaviour
{
    Transform meteorite_ball;
    Transform translucent_ball;
    Transform white_light_in;
    Transform white_light_out;
    Transform white_screen;
    Transform thick_cylinder;
    Transform black_floor;
    Transform yellow_ring;
    Transform yellow_meteorite;
    Transform magma_bubble;
    Transform leafPrefab;
    Color white = Color.white;

    Color ringColor;
    bool isRing;
    float ringTime;

    Color bubbleColor;
    bool isBubble;
    float bubbleTime;

    AudioSource source;
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        white_screen = transform.Find("white_screen");
        yellow_meteorite = transform.Find("yellow_meteorite_floor"); 
        meteorite_ball = transform.Find("go/meteorite_ball");
        translucent_ball = transform.Find("go/translucent_ball");
        white_light_in = transform.Find("white_light_in");
        white_light_out = transform.Find("white_light_out");
        thick_cylinder = transform.Find("thick_cylinder");
        black_floor = transform.Find("black_floor");
        yellow_ring = transform.Find("yellow_ring");
        magma_bubble = transform.Find("Magma_bubble");
        leafPrefab = transform.Find("yezi");
        white.a = 0;
        translucent_ball.GetComponent<Renderer>().material.color= white;
        white_screen.GetComponent<Renderer>().material.color = white;
        bubbleColor=magma_bubble.GetComponent<Renderer>().material.color;
        bubbleColor.a = 0;
        ringColor = yellow_ring.GetComponent<Renderer>().material.color;
        ringColor.a = 0;
    }

    public void SetInit(SkillItem item,float hurt)
    {
        gameObject.GetComponent<SkillHurt>().SetInit(item, hurt);
        ringTime = 0;
        meteorite_ball.localPosition = new Vector3(0,40,0);
        translucent_ball.localPosition = new Vector3(0,40,0);
        thick_cylinder.localScale = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(MoonAnim());
    }
    private void Update()
    {
        if(isRing)
        {
            ringTime += Time.deltaTime;
            if (ringTime > 0.3f)
            {
                ringTime = 0;
                StartCoroutine(RingAnim());
            }
        }

        if(isBubble)
        {
            bubbleTime += Time.deltaTime;
            if(bubbleTime > 0.1f)
            {
                bubbleTime = 0;
                int ran = Random.Range(3, 8);
                for (int i = 0; i < ran; i++)
                {
                    var bubble = Instantiate(magma_bubble);//ObjectPool.Instance.CreateObject("bubble", magma_bubble.gameObject).transform;
                    bubble.gameObject.SetActive(true);
                    bubble.SetParent(transform);
                    bubble.localScale = Vector3.zero;
                    bubble.localPosition = new Vector3(Random.Range(-7, 7), 0, Random.Range(-6, 30));
                    bubble.GetComponent<Renderer>().material.color = bubbleColor;
                    StartCoroutine(BubbleAnim(bubble));
                }
            }
        }
    }
    IEnumerator MoonAnim()
    {
        meteorite_ball.DOLocalMoveY(-0.3f,0.2f);
        translucent_ball.DOLocalMoveY(-0.3f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlaySource("skill_10", source);
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(Recombination(i));
        }
        for (int i = 0; i < 3; i++)
        {
            var ball = Instantiate(translucent_ball);
            ball.SetParent(translucent_ball.parent);
            ball.localPosition = translucent_ball.localPosition;
            StartCoroutine(Translucent(ball,0.2f*(i+1)));
        }
        white_screen.GetComponent<Renderer>().material.DOFade(0.8f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = true;
        isBubble = true;
        white_screen.GetComponent<Renderer>().material.DOFade(0,0.5f);
        StartCoroutine(CylinderAnim());
        float anjle_0 = 0; float dis = 35;
        for (int i = 0; i < 3; i++)
        {
            anjle_0 += 8; dis -= 10;
            StartCoroutine(CreateCubeAngle30(i * 0.1f, anjle_0, dis));
        }
        yellow_meteorite.DOScale(new Vector3(80,0.01f, 80),0.2f);
        yellow_meteorite.GetComponent<Renderer>().material.DOFade(1f,0.2f);
        CreateLeaf();
        yield return new WaitForSeconds(0.5f);
        yellow_meteorite.DOScale(new Vector3(100, 0.01f, 100),2f);
        yellow_meteorite.GetComponent<Renderer>().material.DOFade(0f, 2f);
        yield return new WaitForSeconds(1f);
        meteorite_ball.DOLocalMoveY(-10f, 0.5f);
        translucent_ball.DOLocalMoveY(-10f, 0.5f);
        isBubble = false;
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
        //ObjectPool.Instance.CollectObject(gameObject);
        //ObjectPool.Instance.Clear(leafPrefab.name);
        //ObjectPool.Instance.Clear("bubble");
        //ObjectPool.Instance.Clear("floor_10");
        //ObjectPool.Instance.Clear("ring_10");
        //ObjectPool.Instance.Clear("floor_10");
    }
    #region  圆柱
    IEnumerator CylinderAnim()
    {
        isRing = true;
        thick_cylinder.GetComponent<Renderer>().material.DOFade(0.7f,0.1f);
        thick_cylinder.DOScale(new Vector3(13.2f,30, 13.2f),0.1f);
        yield return new WaitForSeconds(0.1f);
        thick_cylinder.DOScale(new Vector3(12, 30, 12), 0.2f);
        yield return new WaitForSeconds(0.2f);
        thick_cylinder.DOScale(new Vector3(14.4f, 30, 14.4f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        thick_cylinder.DOScale(new Vector3(18, 30, 18),2f);
        thick_cylinder.GetComponent<Renderer>().material.DOFade(0,2);
        isRing = false;
    }
    #endregion
    #region  圆环
    IEnumerator RingAnim()
    {
        var ring = Instantiate(yellow_ring);//ObjectPool.Instance.CreateObject("ring_10", yellow_ring.gameObject).transform;
        ring.gameObject.SetActive(true);
        ring.SetParent(transform);
        ring.localScale = Vector3.zero;
        ring.localPosition = yellow_ring.localPosition;
        ring.GetComponent<Renderer>().material.color= ringColor;
        ring.DOScale(new Vector3(800,500,800), 0.2f);
        ring.DOLocalMoveY(25, 1.2f);
        ring.GetComponent<Renderer>().material.DOFade(0.7f,0.2f);
        yield return new WaitForSeconds(0.2f);
        ring.DOScale(new Vector3(1000, 800, 1000), 1f);
        ring.GetComponent<Renderer>().material.DOFade(0,1f);
        yield return new WaitForSeconds(1f);
        ring.gameObject.SetActive(false);
    }
    #endregion
    #region  地面圆圈
    private Vector3 centerPos;    //你围绕那个点 就用谁的角度
    private float angle = 0;      //偏移角度 
    IEnumerator CreateCubeAngle30(float interval,float ang,float dic)
    {
        yield return new WaitForSeconds(interval);
        centerPos = meteorite_ball.localPosition;
        Vector3 point = thick_cylinder.localPosition;
        point.y -= 15;
        for (angle = 0; angle < 360; angle += ang)
        {
            float x = centerPos.x + dic * Mathf.Cos(angle * 3.14f / 180f);
            float y = centerPos.y + dic * Mathf.Sin(angle * 3.14f / 180f);
            var floor = Instantiate(black_floor); //ObjectPool.Instance.CreateObject("floor_10", black_floor.gameObject).transform;
            floor.gameObject.SetActive(true);
            floor.SetParent(meteorite_ball.parent, true);
            floor.localPosition = new Vector3(x, centerPos.z, y);
            float ran = Random.Range(2f, 4f);
            floor.localScale = new Vector3(ran, 0, ran);
            floor.LookAt(point);
            StartCoroutine(FloorAnim(floor, Random.Range(15,25)));
        }
    }
    IEnumerator FloorAnim(Transform floor, float size)
    {
        floor.DOScaleY(size, 0.05f);
        yield return new WaitForSeconds(0.1f);
        floor.DOScaleY(size * 1.1f, 0.5f);
        yield return new WaitForSeconds(1f);
        floor.DOScaleY(0, 1);
        floor.DOLocalMoveY(-5,1);
        // floor.GetComponent<Renderer>().material.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        floor.gameObject.SetActive(false);
    }
    #endregion
    #region  泡泡
    IEnumerator BubbleAnim(Transform bubble)
    {
        bubble.DOScale(Vector3.one * Random.Range(0.3f,0.5f),0.5f);
        bubble.GetComponent<Renderer>().material.DOFade(1, 1);
        bubble.DOLocalMoveY(25,2.5f);
        yield return new WaitForSeconds(0.5f);
        bubble.DOScale(Vector3.one * Random.Range(0.5f,1f),2);
        bubble.GetComponent<Renderer>().material.DOFade(0,2);
        yield return new WaitForSeconds(2);
        bubble.gameObject.SetActive(false);
    }
    #endregion
    #region  半透明球
    IEnumerator Translucent(Transform tran,float interval)
    {
        //第一段
        tran.localScale = Vector3.one * 12;
        tran.DOScale(Vector3.one*13.2f, 0.1f);
        tran.GetComponent<Renderer>().material.DOFade(0.8f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        tran.DOScale(Vector3.one * 48, interval);
        tran.GetComponent<Renderer>().material.DOFade(0.3f, interval);
        yield return new WaitForSeconds(interval);
        tran.GetComponent<Renderer>().material.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tran.gameObject.SetActive(false);
    }
    #endregion
    #region  光柱复合体
    WaitForSeconds wait = new WaitForSeconds(0.2f);
    Vector3 angle1_0 = new Vector3(-8,-4,10);
    Vector3 angle1_1 = new Vector3(-45,-10,30);
    Vector3 angle2_0 = new Vector3(-75, 0, 2);
    Vector3 angle2_1 = new Vector3(-75, 3.5f, -5);
    Vector3 angle3_0 = new Vector3(-20, 25, -50);
    Vector3 angle3_1 = new Vector3(-35, 40, -45);
    IEnumerator Recombination(float index)
    {
        var light_out = Instantiate(white_light_out);//ObjectPool.Instance.CreateObject(white_light_out.name, white_light_out.gameObject).transform;
        var light_in = Instantiate(white_light_in);//ObjectPool.Instance.CreateObject(white_light_in.name, white_light_in.gameObject).transform;
        light_out.gameObject.SetActive(true);
        light_in.gameObject.SetActive(true);
        light_out.SetParent(transform);
        light_in.SetParent(transform);
        light_out.localEulerAngles = Vector3.zero;
        light_in.localEulerAngles = Vector3.zero;
        white.a = 0.7f;
        light_in.GetComponent<Renderer>().material.color= white;
        white.a = 0.5f;
        light_out.GetComponent<Renderer>().material.color= white;
        light_in.localScale = new Vector3(0, 30, 0);
        yield return wait;
        if (index == 0)
        {
            light_out.localScale = new Vector3(0, 30,0);
            light_in.DORotate(angle1_0, 0.2f);
            light_out.DORotate(angle1_0, 0.2f);
            light_in.DOScale(new Vector3(1f, 50, 1f), 0.2f);
            light_out.DOScale(new Vector3(3.9f, 50, 3.9f), 0.2f);
            yield return wait;
            light_in.DORotate(angle1_1, 0.2f);
            light_out.DORotate(angle1_1, 0.2f);
            light_in.DOScale(new Vector3(1.1f, 50, 1.1f), 0.2f);
            light_out.DOScale(new Vector3(4.2f, 50, 4.2f), 0.2f);
        }
        else if(index == 1)
        {
            light_out.localScale = new Vector3(3.9f, 30, 3.9f);
            light_in.DORotate(angle2_0, 0.2f);
            light_out.DORotate(angle2_0, 0.2f);
            light_in.DOScale(new Vector3(1.3f, 50, 1.3f), 0.2f);
            light_out.DOScale(new Vector3(4.2f, 50, 4.2f), 0.2f);
            yield return wait;
            light_in.DORotate(angle2_1, 0.2f);
            light_out.DORotate(angle2_1, 0.2f);
            light_in.DOScale(new Vector3(1.4f, 50, 1.4f), 0.2f);
            light_out.DOScale(new Vector3(4.5f, 50, 4.5f), 0.2f);
        }
        else
        {
            light_out.localScale = new Vector3(3.9f, 30, 3.9f);
            light_in.DORotate(angle3_0, 0.2f);
            light_out.DORotate(angle3_0, 0.2f);
            light_in.DOScale(new Vector3(1.3f, 50, 1.3f), 0.2f);
            light_out.DOScale(new Vector3(4.2f, 50, 4.2f), 0.2f);
            yield return wait;
            light_in.DORotate(angle3_1, 0.2f);
            light_out.DORotate(angle3_1, 0.2f);
            light_in.DOScale(new Vector3(1.4f, 50, 1.4f), 0.2f);
            light_out.DOScale(new Vector3(4.5f, 50, 4.5f), 0.2f);
        }
        yield return new WaitForSeconds(0.2f);
        light_out.GetComponent<Renderer>().material.DOFade(0, 0.5f);
        light_in.GetComponent<Renderer>().material.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        light_out.gameObject.SetActive(false);
        light_in.gameObject.SetActive(false);
    }
    #endregion
    #region 叶子
    void CreateLeaf()
    {
        for (int i = 0; i < 20; i++)
        {
            StartCoroutine(HideLeaf(i*0.3f));
        }
    }
    IEnumerator HideLeaf(float interval)
    {
        yield return new WaitForSeconds(interval);
        var leaf = Instantiate(leafPrefab);//ObjectPool.Instance.CreateObject(leafPrefab.name, leafPrefab.gameObject);
        leaf.gameObject.SetActive(true);
        leaf.transform.SetParent(transform);
        leaf.transform.localScale = Vector3.one * Random.Range(20, 60);
        leaf.transform.localEulerAngles = new Vector3(Random.Range(-50, -15), 0, 0);
        leaf.transform.localPosition = new Vector3(Random.Range(-7, 8), 30, Random.Range(30, 20));
        leaf.transform.DOLocalMoveY(0, 0.5f);
        leaf.transform.DOLocalMoveZ(-20, 1);
        yield return new WaitForSeconds(1);
        leaf.gameObject.SetActive(false);
    }
    #endregion
}
