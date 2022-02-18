using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SkillSnake : MonoBehaviour
{
    private Transform snake_Object;
    private Transform ball_Object1;
    private Transform ball_Object2;
    private Transform smoke_Object;
    private ParticleSystem particle;

    private Material ballmaterial_1;
    private Material ballmaterial_2;

    private Color color_ball;
    private AudioSource source;
    private void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        snake_Object = transform.Find("pCube5");
        ball_Object1 = transform.Find("Sphere1");
        ball_Object2 = transform.Find("Sphere2");
        smoke_Object = transform.Find("smoke");
        particle = transform.Find("SpinPortalRed").GetComponent<ParticleSystem>();
        ballmaterial_1= ball_Object1.GetComponent<Renderer>().material;
        ballmaterial_2 = ball_Object2.GetComponent<Renderer>().material;
        color_ball = ballmaterial_1.color;
        color_ball.a = 0.6f;
    }
    
    public void SetInit(float hurt)
    {
        snake_Object.localPosition = Vector3.up * 35;
        ball_Object1.localScale = Vector3.zero;
        ball_Object2.localScale = Vector3.zero;
        ballmaterial_1.color = color_ball;
        ballmaterial_2.color = color_ball;
        particle.Play();
        CreateModel.Instance.snakeHurt = hurt * 0.01f;
        StartCoroutine(OpenAnimal());
    }

    private IEnumerator OpenAnimal()
    {
        snake_Object.DOLocalMoveY(1,0.5f);
        particle.transform.DOScale(Vector3.one * 8,0.5f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySource("frog_2", source);
        ball_Object1.DOScale(Vector3.one*15,0.5f);
        yield return new WaitForSeconds(0.5f);
        ball_Object1.DOScale(Vector3.one * 30, 0.5f);
        ball_Object2.DOScale(Vector3.one * 15, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ballmaterial_1.DOFade(0,0.5f);
        ball_Object2.DOScale(Vector3.one * 30, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ballmaterial_2.DOFade(0,0.5f);
        snake_Object.DOLocalMoveY(-5, 1f);
        particle.transform.DOScale(Vector3.zero, 0.5f);
        CreateModel.Instance.CreateSnake();
        yield return new WaitForSeconds(1);
        GameObject.Destroy(gameObject);
        //particle.Stop();
        //gameObject.SetActive(false);
    }

    IEnumerator CreateSmoke()
    {
        yield return new WaitForSeconds(0.5f);
        var smoke = Instantiate(smoke_Object);//ObjectPool.Instance.CreateObject("skill16smoke", smoke_Object.gameObject);
        smoke.transform.parent = transform;
        smoke.transform.localPosition = Vector3.forward * Random.Range(-15, 15);
        smoke.transform.localScale = Vector3.one * Random.Range(500, 800);
        yield return new WaitForSeconds(0.5f);
        smoke.transform.DOLocalMoveY(25,1f);
        yield return new WaitForSeconds(1f);
        smoke.gameObject.SetActive(false);
    }

}
