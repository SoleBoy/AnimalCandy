using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Electric_Bullet : MonoBehaviour
{
    private Transform ball;
    private Transform quantum_ball;//发光球
    private Transform quantum_ring;//环
    private Transform quantum_beam;//光束
    private Transform quantum_shell;//外光束

    private bool isInit;
    private bool isBall;
    private float distance_max;

    private Vector3 eulerAngles;
    private Vector3 shellScale;
    private Vector3 beamScale;
    private Color color_ball;
    private Material material_ball; 

    private WaitForSeconds wait = new WaitForSeconds(0.1f);
    void Awake()
    {
        isInit = true;
           ball = transform.Find("ball");
        quantum_ball = transform.Find("quantum_ball");
        quantum_ring = transform.Find("quantum_ring");
        quantum_beam = transform.Find("quantum_beam");
        quantum_shell = transform.Find("quantum_shell");
        material_ball = quantum_ball.GetComponent<Renderer>().material;
        color_ball = material_ball.color;
        color_ball.a = 0.6f;
        ball.GetComponent<Renderer>().material.DOFade(0.6f,0);
        beamScale = new Vector3(0.2f,1f,0.2f);
        shellScale = new Vector3(0.4f, 1f, 0.4f);
        eulerAngles = Vector3.left * 90;
    }

    public void OpenAnimal(RACEIMG state, ShooterItem data, float damage,float distance)
    {
        if(isInit)
        {
            isInit = false;
            distance_max = distance * 0.5f;
            beamScale.y = distance_max;
            shellScale.y = distance_max;
        }
        material_ball.color = color_ball;
        quantum_ball.localScale = Vector3.zero;
        quantum_shell.localPosition = Vector3.zero;
        quantum_beam.localPosition= Vector3.zero;
        quantum_shell.GetComponent<BulletType>().SetTarget(state,data,damage,distance);
        StartCoroutine(QuantumAnim());
    }

    IEnumerator QuantumAnim()
    {
        isBall = true;
        ball.DOScale(Vector3.one * 0.2f, 0.2f);
        StartCoroutine(BallAnim());
        yield return new WaitForSeconds(0.3f);
        quantum_beam.DOLocalMoveZ(distance_max, 0.2f);
        quantum_beam.DOScale(beamScale, 0.2f);
        yield return wait;
        for (int i = 1; i < 4; i++)
        {
            StartCoroutine(RingAnim(i));
        }
        StartCoroutine(ShellAnim());
        material_ball.DOFade(0.4f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        isBall = false;
        ball.DOScale(Vector3.zero, 0.2f);
        quantum_beam.DOScale(Vector3.zero, 0.3f);
        quantum_ball.DOScale(Vector3.zero, 0.3f);
        material_ball.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    //球动画
    IEnumerator BallAnim()
    {
        quantum_ball.DOScale(Vector3.one * 0.4f, 0.1f);
        material_ball.DOFade(0.3f, 0.1f);
        yield return wait;
        quantum_ball.DOScale(Vector3.one * 0.5f, 0.1f);
        material_ball.DOFade(0.6f, 0.1f);
        yield return wait;
        if (isBall)
            StartCoroutine(BallAnim());
    }
    //环动画
    IEnumerator RingAnim(int index)
    {
        yield return new WaitForSeconds(index * 0.1f);
        var ring = ObjectPool.Instance.CreateObject(quantum_ring.name, quantum_ring.gameObject).transform;
        ring.SetParent(transform);
        ring.localPosition = Vector3.forward * index * 1.5f;
        ring.localScale = Vector3.one * 40;
        ring.localEulerAngles = eulerAngles;
        Material material = ring.GetComponent<Renderer>().material;
        material.DOFade(0.6f, 0.1f);
        yield return wait;
        ring.DOScale(Vector3.one * 35, 0.1f);
        material.DOFade(0.4f, 0.1f);
        yield return wait;
        ring.DOScale(Vector3.one * 46, 0.1f);
        material.DOFade(0.3f, 0.1f);
        yield return wait;
        ring.DOScale(Vector3.one * 50, 0.1f);
        material.DOFade(0.5f, 0.1f);
        yield return wait;
        ring.DOScale(Vector3.one * 35, 0.1f);
        material.DOFade(0.6f, 0.1f);
        yield return wait;
        ring.DOScale(Vector3.zero, 0.1f);
        material.DOFade(0f, 0.1f);
        yield return wait;
        ring.gameObject.SetActive(false);
    }
    //外环动画
    IEnumerator ShellAnim()
    {
        quantum_shell.DOLocalMoveZ(distance_max, 0.2f);
        quantum_shell.DOScale(shellScale, 0.3f);
        yield return new WaitForSeconds(0.3f);
        quantum_shell.DOScaleX(0.2f, 0.1f);
        quantum_shell.DOScaleZ(0.2f, 0.1f);
        yield return wait;
        quantum_shell.DOScaleX(0.5f, 0.1f);
        quantum_shell.DOScaleZ(0.5f, 0.1f);
        yield return wait;
        quantum_shell.DOScaleX(0.2f, 0.1f);
        quantum_shell.DOScaleZ(0.2f, 0.1f);
        yield return wait;
        quantum_shell.DOScale(Vector3.zero, 0.3f);
    }
}
