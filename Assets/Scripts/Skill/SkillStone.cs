using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 技能石壁
/// </summary>
public class SkillStone : MonoBehaviour
{
    Transform piecesPrefab;
    Vector3 scale;
    Vector3 scaleInit;
    float lastTime;
    bool isPieces;
    AudioSource source;
    void Awake()
    {
        if (!source)
            source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        piecesPrefab = Resources.Load<Transform>("Effects/PixelBlock");
        scale = piecesPrefab.localScale;
        scaleInit = new Vector3(1,1,0);
        if (GameManager.Instance.modeSelection == "roude")
        {
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
    private void Update()
    {
        if(isPieces)
        {
            lastTime += Time.deltaTime;
            if (lastTime >= 0.2f)
            {
                lastTime = 0;
                for (int i = 0; i < 20; i++)
                {
                    var spray = Instantiate(piecesPrefab);//ObjectPool.Instance.CreateObject("PixelBlock", piecesPrefab.gameObject);
                    spray.gameObject.SetActive(true);
                    spray.transform.SetParent(transform);
                    spray.transform.localScale = scale * Random.Range(0.3f, 1f);
                    spray.GetComponent<Renderer>().material.color = Color.red;
                    spray.transform.localPosition = new Vector3(Random.Range(-4,4),6.8f, Random.Range(transform.localPosition.z, 90));
                    spray.transform.GetComponent<PixelBlock>().SetPower(Random.Range(5, 10));
                }
            }
        }
    }
    public void SetInit(SkillItem item,float hurt)
    {
        AudioManager.Instance.PlaySource("skill_5", source);
        isPieces = true;
        GetComponent<SkillHurt>().SetInit(item, hurt);
        transform.localScale = scaleInit;
        StartCoroutine(SelfAnim());
    }
    
    IEnumerator SelfAnim()
    {
        transform.DOScaleZ(1, 0.5f);
        yield return new WaitForSeconds(1);
        transform.DOLocalMoveY(-10, 1f);
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
        //isPieces = false;
        //ObjectPool.Instance.CollectObject(gameObject);
    }
}
