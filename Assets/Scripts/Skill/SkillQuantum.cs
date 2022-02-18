using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能电磁炮
/// </summary>
public class SkillQuantum : MonoBehaviour
{
    Transform prefab;
    Transform ball;
    Transform quantum_ball;//发光球
    Transform quantum_ring;//环
    Transform quantum_beam;//光束
    Transform quantum_shell;//外光束

    Vector3 shellScale;
    bool isBall;
    AudioSource source;
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    WaitForSeconds rings = new WaitForSeconds(0.2f);
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        prefab = Resources.Load<Transform>("Effects/PixelBlock");
        ball = transform.Find("ball");
        quantum_ball = transform.Find("quantum_ball");
        quantum_ring = transform.Find("quantum_ring");
        quantum_beam = transform.Find("quantum_beam");
        quantum_shell = transform.Find("quantum_shell");
        shellScale = quantum_shell.localScale;
        ball.GetComponent<Renderer>().material.DOFade(0.8f, 0.2f);
    }
    public void SetInit(SkillItem item,float hurt)
    {
        quantum_shell.GetComponent<SkillHurt>().SetInit(item,hurt);
        quantum_ball.localScale = Vector3.zero;
        quantum_ball.GetComponent<Renderer>().material.DOFade(1, 0);
        StartCoroutine(QuantumAnim());
    }
    IEnumerator QuantumAnim()
    {
        isBall = true;
        ball.DOScale(Vector3.one*4,0.2f);
        StartCoroutine(BallAnim());
        AudioManager.Instance.PlaySource("skill_8_1", source);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySource("skill_8_2", source);
        quantum_beam.DOLocalMoveZ(92, 0.2f);
        quantum_beam.DOScale(new Vector3(1,100,1),0.2f);
        yield return rings;
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(RingAnim(i));
        }
        StartCoroutine(ShellAnim());
        quantum_shell.GetComponent<Collider>().enabled = true;
        quantum_shell.GetComponent<Renderer>().material.DOFade(0.8f,1.5f);
        yield return new WaitForSeconds(2f);
        isBall = false;
        quantum_beam.DOScale(Vector3.zero, 0.5f);
        quantum_beam.DOLocalMoveZ(-7, 0.5f);
        ball.DOScale(Vector3.zero, 0.2f);
        quantum_ball.DOScale(Vector3.zero, 0.5f);
        quantum_ball.GetComponent<Renderer>().material.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(gameObject);
        //quantum_shell.GetComponent<Collider>().enabled = false;
        //ObjectPool.Instance.CollectObject(gameObject);
        //ObjectPool.Instance.Clear("SkillQuantum");
    }
    //球动画
    IEnumerator BallAnim()
    {
        quantum_ball.DOScale(Vector3.one * 6, 0.2f);
        quantum_ball.GetComponent<Renderer>().material.DOFade(0.6f, 0.2f);
        yield return rings;
        quantum_ball.DOScale(Vector3.one * 4f, 0.2f);
        quantum_ball.GetComponent<Renderer>().material.DOFade(0.8f, 0.2f);
        yield return rings;
        if(isBall)
            StartCoroutine(BallAnim());
    }
   //环动画
    IEnumerator RingAnim(int index)
    {
        yield return new WaitForSeconds(index * 0.1f);
        var ring = Instantiate(quantum_ring);//ObjectPool.Instance.CreateObject(quantum_ring.name,quantum_ring.gameObject).transform;
        ring.gameObject.SetActive(true);
        ring.SetParent(transform);
        ring.localPosition = new Vector3(0,5,5*index);
        ring.localScale = Vector3.one * 300;
        ring.GetComponent<Renderer>().material.DOFade(0.8f, 0f);
        QuakePiecces(ring);
        yield return rings;
        ring.DOScale(Vector3.one*310, 0.2f);
        ring.GetComponent<Renderer>().material.DOFade(0.5f, 0.2f);
        yield return rings;
        ring.DOScale(Vector3.one * 300, 0.2f);
        ring.GetComponent<Renderer>().material.DOFade(0.3f, 0.2f);
        yield return rings;
        ring.DOScale(Vector3.one * 320, 0.2f);
        ring.GetComponent<Renderer>().material.DOFade(0.6f, 0.2f);
        yield return new WaitForSeconds(0.7f);
        ring.DOScale(Vector3.one * 270, 0.2f);
        ring.GetComponent<Renderer>().material.DOFade(0.8f, 0.2f);
        yield return rings;
        ring.DOScale(Vector3.zero, 0.2f);
        ring.GetComponent<Renderer>().material.DOFade(0f, 0.2f);
        yield return rings;
        ring.gameObject.SetActive(false);
    }
    //外环动画
    IEnumerator ShellAnim()
    {
        quantum_shell.DOScale(new Vector3(5,100,5), 0.3f);
        yield return new WaitForSeconds(0.3f);
        quantum_shell.DOScale(new Vector3(2.5f, 100, 2.5f), 0.1f);
        yield return wait;
        quantum_shell.DOScale(new Vector3(5, 100, 5), 0.1f);
        yield return wait;
        quantum_shell.DOScale(new Vector3(4, 100, 4), 0.1f);
        yield return wait;
        quantum_shell.DOScale(new Vector3(1.5f, 100, 1.5f), 0.1f);
        yield return wait;
        quantum_shell.DOScale(new Vector3(0, 100, 0), 0.3f);
    }
    //小碎块
    void QuakePiecces(Transform ring)
    {
        for (int i = 0; i < 15; i++)
        {
            var spray = Instantiate(prefab);//ObjectPool.Instance.CreateObject("SkillQuantum", prefab.gameObject);
            spray.gameObject.SetActive(true);
            spray.transform.SetParent(transform);
            spray.transform.localScale = prefab.localScale * Random.Range(0.5f, 1f);
            spray.GetComponent<Renderer>().material.color = ring.GetComponent<Renderer>().material.color;
            spray.transform.localEulerAngles = new Vector3(0, 0,Random.Range(-50,50));
            Vector3 point = ring.localPosition;
            point.x += Random.Range(-1f,1f);
            spray.transform.localPosition = point;
            spray.transform.GetComponent<PixelBlock>().SetPower(Random.Range(10, 15));
        }
    }
}
